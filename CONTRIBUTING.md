# Contributing to WigMaker

Thanks for your interest in contributing to WigMaker! This guide will help you get started.

## How to Contribute

### Reporting Issues

- Search [existing issues](https://github.com/QuinntyneBrown/WigMaker/issues) before opening a new one
- Include steps to reproduce the problem
- Describe the expected vs actual behavior
- Include relevant environment details (OS, browser, Playwright version)

### Suggesting Features

- Open a [GitHub issue](https://github.com/QuinntyneBrown/WigMaker/issues/new) describing the feature
- Explain the use case and why it would be valuable
- Include examples of how the feature would work if possible

### Submitting Changes

1. **Fork** the repository
2. **Create a branch** from `main`:
   ```bash
   git checkout -b feature/your-feature-name
   ```
3. **Make your changes** and commit with clear, descriptive messages
4. **Push** your branch:
   ```bash
   git push origin feature/your-feature-name
   ```
5. **Open a Pull Request** against `main`

## Development Setup

```bash
git clone https://github.com/QuinntyneBrown/WigMaker.git
cd WigMaker
playwright-cli install-browser
```

## Guidelines

### Commit Messages

- Use the imperative mood ("Add feature" not "Added feature")
- Keep the first line under 72 characters
- Reference related issues when applicable (e.g., `Fixes #12`)

### Documentation

- Update relevant reference docs in `.claude/skills/playwright-cli/references/` when changing skill behavior
- Keep `SKILL.md` in sync with any command changes
- Use clear, concise language with practical examples

### Code of Conduct

- Be respectful and constructive in discussions
- Welcome newcomers and help them get started
- Focus feedback on the work, not the person

## Questions?

Open an issue or start a discussion on the [GitHub repository](https://github.com/QuinntyneBrown/WigMaker).
