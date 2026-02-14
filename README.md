# WigMaker

A Claude Code skill definition for browser automation using Playwright CLI. WigMaker provides a ready-to-use skill that enables automated browser interactions, web testing, form filling, screenshots, and data extraction directly from the Claude Code CLI.

## Features

- **Browser Automation** - Open, navigate, and interact with web pages across Chrome, Firefox, WebKit, and Edge
- **Form Interaction** - Fill inputs, click buttons, and submit forms
- **Screenshots & Video** - Capture screenshots and record browser sessions
- **Network Mocking** - Intercept and mock network requests
- **Session Management** - Manage multiple browser sessions with persistent profiles
- **Storage State** - Handle cookies, localStorage, and sessionStorage
- **Tracing** - Debug and profile browser interactions
- **Test Generation** - Generate Playwright tests from recorded actions

## Getting Started

### Prerequisites

- [Claude Code CLI](https://claude.com/claude-code)
- Playwright CLI

### Installation

```bash
git clone https://github.com/QuinntyneBrown/WigMaker.git
cd WigMaker
```

Install Playwright browsers:

```bash
playwright-cli install-browser
```

### Basic Usage

```bash
# Open a browser and navigate
playwright-cli open https://example.com

# Take a screenshot
playwright-cli screenshot

# Fill a form field
playwright-cli fill e1 "user@example.com"

# Click an element
playwright-cli click e2

# Close the browser
playwright-cli close
```

### Advanced Usage

**Network request mocking:**

```bash
playwright-cli route "**/*.jpg" --status=404
playwright-cli route "**/api/users" --body='[{"name": "John"}]'
```

**Browser sessions:**

```bash
playwright-cli -s=mysession open example.com --persistent
```

**Video recording:**

```bash
playwright-cli video-start
# ... perform actions ...
playwright-cli video-stop
```

**Tracing:**

```bash
playwright-cli tracing-start
# ... perform actions ...
playwright-cli tracing-stop
```

## Project Structure

```
WigMaker/
├── .claude/skills/playwright-cli/   # Playwright CLI skill definition
│   ├── SKILL.md                     # Main skill documentation
│   └── references/                  # Reference guides
│       ├── request-mocking.md
│       ├── running-code.md
│       ├── session-management.md
│       ├── storage-state.md
│       ├── test-generation.md
│       ├── tracing.md
│       └── video-recording.md
└── .playwright/                     # Playwright runtime directory
```

## License

See [LICENSE](LICENSE) for details.
