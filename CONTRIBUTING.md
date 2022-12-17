## Contributing

#### Contributions are more than welcome! Please work in the dev branch to do so:

- Create or update your own fork of CadPythonShell under your GitHub account.
- Clone project with command :`git clone https://github.com/chuongmep/CadPythonShell.git --recurse-submodules`
- Checkout to the ``dev`` branch.
- In the dev branch, implement and test you changes specific to the feature.
- Build the project and make sure everything works.
- Create well-documented commits of your changes.
- Update Submodule: 
  -  git submodule update --remote
- Commit submodules in case change in submodule
  - cd path/to/submodule
  - git add <stuff>
  - git commit -m "comment"
  - git push
  - Create a pull request to submodules
- Update and commit submodule: git submodule update --remote
- Submit a pull request to the origin:dev branch.

#### Please avoid:

- Lots of unrelated changes in one commit.
- Modifying files that are not directly related to the feature you implement.
