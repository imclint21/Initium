# Contributing to Initium

Thank you for your interest in contributing to **Initium**! We value contributions from the community, whether it's fixing bugs, adding features, improving documentation, or suggesting new ideas. Please follow these guidelines to ensure a smooth collaboration process.

---

## Getting Started

### 1. Fork the Repository
- Navigate to the [Initium repository](https://github.com/imclint21/Initium).
- Click the **Fork** button in the top-right corner to create a copy of the repository in your GitHub account.

### 2. Clone Your Fork
```bash
git clone https://github.com/yourusername/Initium.git
```
Replace `yourusername` with your GitHub username.

### 3. Set Up the Development Environment
- Ensure you have the following installed:
  - .NET 6 SDK or higher
  - Git
- Navigate to the project folder and restore dependencies:
  ```bash
  cd Initium
  dotnet restore
  ```

### 4. Create a New Branch
Use the following branch naming convention for new features:
```bash
git checkout -b features/<feature-name>
```
Replace `<feature-name>` with a brief description of the feature or fix.

---

## Making Changes

### Code Guidelines
- Follow the project's existing code style and organization.
- Include comments to explain complex code and logic.
- Write unit tests for any new features or bug fixes.

### Running Tests
Run the test suite to ensure your changes don't break existing functionality:
```bash
dotnet test
```

---

## Submitting Your Contribution

### 1. Commit Your Changes
Use clear and concise commit messages:
```bash
git add .
git commit -m "Add feature: <feature-name>"
```

### 2. Push to Your Fork
Push your changes to the branch you created:
```bash
git push origin features/<feature-name>
```

### 3. Open a Pull Request
- Go to your fork on GitHub.
- Click the **New Pull Request** button.
- Select your branch and compare it with the `main` branch of the original repository.
- Provide a descriptive title and summary of your changes.


## Questions or Suggestions?

If you have any questions or suggestions, feel free to open an issue or reach out via GitHub discussions. We appreciate your feedback and contributions!

Thank you for helping to make Initium better!
