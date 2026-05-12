<p align="center">
  <h1 align="center">ResumeUps</h1>
  <p align="center">
    <b>An AI tool for resume analysis and matching system</b>
    <br/>
    <i>Extract resume data · Analyze with LLMs</i>
  </p>
</p>

<p align="center">
  <img src="https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet" alt=".NET 10" />
  <img src="https://img.shields.io/badge/Angular-21.2-DD0031?logo=angular" alt="Angular 21" />
  <img src="https://img.shields.io/badge/license-Apache%20License%202.0-blue" alt="License" />
</p>

---

## What is ResumeUps?

ResumeUps act like an ATS resume screening and analysis system built with .NET 10 and Angular 21. It automates the process of extracting information from resumes (PDF, DOCX,Txt), and using AI to provide deep feedback and matching scores against job descriptions.

**It acts as an Elite HR assistant of big company.** Instead of manually reviewing resumes, you can upload them to the system. ResumeUps parses the text, extracts skills and keywords, and feeds the data into an LLM (using OpenRouter or local models) to analyze alignment with job requirements, providing constructive feedback, identifying missing skills, and highlighting key strengths.

## Important note

This project is currently under heavy development. The AI and prompts are continuously being refined to provide more accurate extraction (especially for non-IT industries) and better feedback. Some features or analysis results may occasionally be inaccurate or require further fine-tuning. 
The information is not fully masked and can be seen by the AI provider if you feel uncomfortable please mask your personal information before uploading the resume. This project doesn't store your data for any purpose. 
For more information please read the [License](LICENSE) and [Disclaimer](LICENSE) files as well as terms of service inside the website.

## Core Features

- **Document Parsing:** Supports `.pdf` and `.docx` using PdfPig and NPOI.
- **Matching System:** Compares resume skills against a specific Job Description and calculates a match score.
- **Feedback System:** Analyzes the resume qualitatively to provide actionable feedback and identify structural or content issues.
- **UI:** Built with Angular 21 and TailwindCSS, featuring smooth GSAP animations and a responsive bento-box design.

---

## How It Works

### 1. Document Extraction
When a user uploads a resume:
- The `ResumeExtractorController` parses the document text.

### 2. Analysis
Once extracted, the text and job description are processed through the `AnalyzeService`:
1. **Matching Service:** Extracts industry-specific skills and keywords from the resume and compares them to the job description to calculate a match score.
2. **Feedback Service:** Evaluates the resume content against the job description to provide categorized issues (severity levels) and actionable suggestions.

### 3. Presentation
The backend returns a comprehensive JSON result, which the Angular frontend visualizes using an interactive, modern interface with interactive cards and GSAP animations.

---

## Getting Started ( For developers )

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Node.js](https://nodejs.org/) & npm (for Angular frontend) or *[bun](https://bun.sh/) for effecient package and task runner*.
- LLM API key (e.g., OpenRouter). You can get one for free at [OpenRouter](https://openrouter.ai/keys).

### 1. Clone & Configure

```bash
git clone https://github.com/nupniichan/resumeups.git
cd resumeups
```

Configure your environment variables in the backend:

```bash
cd resumeups.Server
cp .env.example .env
```

**Required variables:**

| Variable | Description |
|----------|-------------|
| `Llm_ApiKey` | LLM provider API key (e.g., OpenRouter) |
| `Llm_ModelName` | Model to use (e.g., google/gemma-4-26b-a4b-it) |

### 2. Run Locally

**Terminal 1 — Backend (.NET WebAPI)**
```bash
cd resumeups.Server
dotnet run
```

**Terminal 2 — Frontend (Angular)**
```bash
cd resumeups.client
npm install
npm start
```

---

## Project Structure

```
resumeups/
├── resumeups.Server/                 # .NET 10 WebAPI Backend
│   ├── Controllers/                  # API Endpoints (ResumeExtractor, ResumeAnalysis)
│   ├── Services/                     # Business Logic (AnalyzeService, MatchingService, FeedbackService)
│   ├── Utils/                        # Document Parsers (PdfPig, NPOI) and PII Masking
│   ├── Models/                       # Shared contracts and AI response formats
│   └── Program.cs                    # Application configuration
├── resumeups.client/                 # Angular 21 Frontend
│   ├── src/                          
│   │   ├── app/                      # Components, Pages (resume-checker.ts, etc.)
│   │   ├── styles.css                # TailwindCSS config
│   │   └── ...                       
│   └── package.json                  # Dependencies (Angular, GSAP, TailwindCSS)
├── LICENSE
└── README.md
```

## Disclaimer

This project is a personal project developed for educational and research purposes. The AI generated analysis and feedback are for reference only and should not be solely relied upon for professional hiring decisions. Always manually review resumes when making final employment choices.

## Acknowledgments

Thanks to the open source community, particularly the teams behind .NET, Angular, PdfPig, NPOI, and GSAP and more. Their incredible tools make projects like this possible. ❤️

## License

Take a look at [Apache License 2.0](https://github.com/nupniichan/resumeups/blob/main/LICENSE)

---

Thanks for visiting my repository, your attention is my pleasure ⸜(｡˃ ᵕ ˂ )⸝♡ 