import os

import chromadb
import streamlit as st

from dotenv import load_dotenv

from llama_index.core import (
    VectorStoreIndex,
    StorageContext,
    Settings
)

from llama_index.llms.openai import OpenAI
from llama_index.embeddings.openai import OpenAIEmbedding

from llama_index.vector_stores.chroma import ChromaVectorStore


# --------------------------------------------------
# Configuration
# --------------------------------------------------

load_dotenv()

if not os.getenv("OPENAI_API_KEY"):
    st.error("OPENAI_API_KEY is not configured.")
    st.stop()


Settings.llm = OpenAI(
    model="gpt-4o-mini",
    temperature=0
)

Settings.embed_model = OpenAIEmbedding(
    model="text-embedding-3-small"
)


# --------------------------------------------------
# Load ChromaDB
# --------------------------------------------------

chroma_client = chromadb.PersistentClient(
    path="./chroma_db"
)

chroma_collection = chroma_client.get_or_create_collection(
    "company_policy"
)


vector_store = ChromaVectorStore(
    chroma_collection=chroma_collection
)

storage_context = StorageContext.from_defaults(
    vector_store=vector_store
)


index = VectorStoreIndex.from_vector_store(
    vector_store,
    storage_context=storage_context
)


# --------------------------------------------------
# Query Engine
# --------------------------------------------------

query_engine = index.as_query_engine(
    similarity_top_k=3
)


# --------------------------------------------------
# Streamlit UI
# --------------------------------------------------

st.set_page_config(
    page_title="Company Policy Assistant",
    page_icon="📚"
)

st.title("📚 Company Policy Assistant")

st.write(
    "Ask questions about the company policies."
)


question = st.text_input(
    "Enter your question:"
)


if question:

    with st.spinner("Searching company documents..."):

        response = query_engine.query(
            question
        )

    st.subheader("Answer")

    st.write(response)