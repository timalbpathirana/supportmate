from flask import Flask, request, jsonify
from langchain.text_splitter import RecursiveCharacterTextSplitter
from langchain_community.embeddings import HuggingFaceEmbeddings
from langchain_pinecone import PineconeVectorStore
from langchain.docstore.document import Document
import os
from dotenv import load_dotenv

load_dotenv()
app = Flask(__name__)

# Initialize HuggingFace embeddings
model_name = "BAAI/bge-large-en-v1.5"
model_kwargs = {"device": "cpu"}
encode_kwargs = {"normalize_embeddings": True}
embedding = HuggingFaceEmbeddings(
    model_name=model_name, model_kwargs=model_kwargs, encode_kwargs=encode_kwargs
)

# The PineconeVectorStore client automatically looks for the PINECONE_API_KEY
index_name = os.getenv("PINECONE_INDEX")

@app.route("/upload", methods=["POST"])
def upload():
    data = request.get_json()
    content = data.get("content")

    if not content:
        return jsonify({"error": "No content provided"}), 400

    docs = [Document(page_content=content)]
    splitter = RecursiveCharacterTextSplitter(chunk_size=500, chunk_overlap=50)
    split_docs = splitter.split_documents(docs)

    # Upsert documents to Pinecone
    PineconeVectorStore.from_documents(
        split_docs, 
        embedding, 
        index_name=index_name
    )

    return jsonify({"message": f"Stored {len(split_docs)} chunks in Pinecone index '{index_name}'."})

@app.route("/retrieve", methods=["POST"])
def retrieve():
    try:
        # Initialize from existing index
        vectorstore = PineconeVectorStore.from_existing_index(index_name, embedding)
    except Exception as e:
        # This will catch errors if the index doesn't exist
        return jsonify({"error": f"Could not load index '{index_name}'. Have you uploaded documents first? Error: {e}"}), 400


    data = request.get_json()
    question = data.get("question")

    if not question:
        return jsonify({"error": "No question provided"}), 400

    results = vectorstore.similarity_search(question, k=3)
    context = "\n".join([doc.page_content for doc in results])

    return jsonify({"context": context})

if __name__ == "__main__":
    app.run(port=5050)
