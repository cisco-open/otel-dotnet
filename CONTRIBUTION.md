# Contributing Guide

We'd love your help!

### Env Prerequisites

Make sure you have the following env versions:

```shell
.NET 6
```

### Fork

In the interest of keeping this repository clean and manageable, you should work from a fork. To create a fork, click the 'Fork' button at the top of the repository, then clone the fork locally using `git clone git@github.com:USERNAME/otel-dotnet.git`.

You should also add this repository as an "upstream" repo to your local copy, in order to keep it up to date. You can add this as a remote like so:

```bash
git remote add upstream https://github.com/cisco-open/otel-dotnet.git

#verify that the upstream exists
git remote -v
```

To update your fork, fetch the upstream repo's branches and commits, then merge your main with upstream's main:

```bash
git fetch upstream
git checkout main
git merge upstream/main
```

#### Setup repo

After the clone you will want to install all repo deps, run:

```bash
dotnet restore
```

This will install the main package deps and then all the scope packages deps (and also will build the sub packages)

To run all tests:

```bash
dotnet test
```
