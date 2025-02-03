# Coding standarts

This section describes practices and guidelines used in this project. The guide describes conventions for backend, frontend and commits messages.

## 1. Backend

## 1.1 Guidelines

- We use standard .NET naming conventions:
  - **PascalCas**e for class names, methods, and properties
  - **camelCase** for private fields and local variables.
  - **ALL_CAPS** for constants.

## 1.2 Project Structure

## 2. Frontend

## 2.1 Guidelines

- **camelCase** for variables and functions
- **PascalCase** for Svelte components

## 2.2 Project Structure

## 3. Commit Message Guidelines (Conventional commits)

We use [Conventional Commits](https://www.conventionalcommits.org/)

### **3.1 Commit Format**

```
<type>(optional scope): <description>

[optional body]

[optional footer]
```

### **3.2 Commit Types**

| Type       | Meaning                                                |
| ---------- | ------------------------------------------------------ |
| `feat`     | A new feature                                          |
| `fix`      | A bug fix                                              |
| `docs`     | Documentation changes                                  |
| `style`    | Formatting, missing semicolons, etc. (no code changes) |
| `refactor` | Code restructuring without feature changes             |
| `test`     | Adding or updating tests                               |
| `chore`    | Maintenance tasks (e.g., CI, dependencies)             |
| `perf`     | Performance improvements                               |
