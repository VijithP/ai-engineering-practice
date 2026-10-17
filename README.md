# ai-engineering-practice

A hands-on collection of Generative AI experiments and practice projects, covering Retrieval-Augmented Generation (RAG), evaluation, guardrails, observability, cost optimization, and chatbot development.

Each folder is a self-contained project that explores one concept or pattern.

## Projects

### RAG and Knowledge Assistants
| Project | Description |
|---|---|
| `company-policy-rag` | RAG app that answers questions over company policy documents |
| `HybridRagApp` | Hybrid retrieval (keyword + vector search) for better answer relevance |
| `HybridRagAppAuto` | Automated variant of the hybrid RAG pipeline |
| `InternalKnowledgeBase` | Ingestion and indexing of internal documents |
| `InternalKnowledgeAssistant` | Assistant layer on top of the internal knowledge base |

### Chatbots and Extraction
| Project | Description |
|---|---|
| `chatbot` | Basic LLM-powered chatbot |
| `SkillChatbot` | Chatbot grounded in skills/profile data |
| `structured-data-extractor` | Extracts structured JSON from unstructured text using LLMs |

### Quality, Safety and Observability
| Project | Description |
|---|---|
| `RagasEvaluation` | Evaluating RAG pipelines with Ragas metrics |
| `GuardrailsAI` | Input/output validation and safety guardrails for LLM apps |
| `LangSmithCSharp` | Tracing and observability with LangSmith from C# |

### Performance and Cost
| Project | Description |
|---|---|
| `PromptCaching` | Reducing latency and cost with prompt caching |
| `CostProfilingExercise` | Profiling token usage and LLM cost |

### APIs
| Project | Description |
|---|---|
| `FastApiAI` | Exposing AI features as REST APIs with FastAPI |

## Tech Stack

- **Languages:** C# / .NET, Python
- **LLMs:** Anthropic Claude, OpenAI
- **Tools:** FastAPI, LangSmith, Ragas, Guardrails AI
- **Concepts:** RAG, hybrid search, embeddings, prompt caching, evaluation, observability

> Update this list to match what each project actually uses.

## Getting Started

```bash
git clone https://github.com/<your-username>/ai-engineering-practice.git
cd ai-engineering-practice/<project-folder>
```

Each project has its own setup. In general:

1. Copy `.env.example` to `.env` and add your API keys.
2. Install dependencies (`dotnet restore` for C# projects, `pip install -r requirements.txt` for Python projects).
3. Run the project (`dotnet run` or `python main.py` / `uvicorn main:app --reload`).

## Repository Structure

```
ai-engineering-practice/
├── chatbot/
├── company-policy-rag/
├── CostProfilingExercise/
├── FastApiAI/
├── GuardrailsAI/
├── HybridRagApp/
├── HybridRagAppAuto/
├── InternalKnowledgeAssistant/
├── InternalKnowledgeBase/
├── LangSmithCSharp/
├── PromptCaching/
├── RagasEvaluation/
├── SkillChatbot/
└── structured-data-extractor/
```

## Notes

- API keys are never committed. Keep secrets in `.env` (already in `.gitignore`).
- These are learning projects, not production-ready code.

## Author

**Vijith P V** - Senior Technical Specialist | .NET, Azure, AWS, Generative AI

## License

MIT
