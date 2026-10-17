import os

import chromadb
from dotenv import load_dotenv

from llama_index.core import (
    SimpleDirectoryReader,
    VectorStoreIndex,
    StorageContext,
    Settings
)

from llama_index.core.node_parser import SentenceSplitter

from llama_index.embeddings.openai import OpenAIEmbedding

from llama_index.vector_stores.chroma import ChromaVectorStore


load_dotenv()

if not os.getenv("OPENAI_API_KEY"):
    raise ValueError("OPENAI_API_KEY is not configured.")


# Configure embedding model
Settings.embed_model = OpenAIEmbedding(
    model="text-embedding-3-small"
)


# Load documents
documents = SimpleDirectoryReader(
    "data"
).load_data()

print(f"Loaded {len(documents)} documents.")


# Split documents into chunks
splitter = SentenceSplitter(
    chunk_size=500,
    chunk_overlap=50
)

nodes = splitter.get_nodes_from_documents(documents)

print(f"Created {len(nodes)} chunks.")


# Create ChromaDB client
chroma_client = chromadb.PersistentClient(
    path="./chroma_db"
)

chroma_collection = chroma_client.get_or_create_collection(
    "company_policy"
)


# Create vector store
vector_store = ChromaVectorStore(
    chroma_collection=chroma_collection
)


storage_context = StorageContext.from_defaults(
    vector_store=vector_store
)


# Create vector index
index = VectorStoreIndex(
    nodes,
    storage_context=storage_context
)


print("Documents successfully indexed.")