---
name: release-checklist
description: 'Prepare a NuGet package release with version bump, changelog, and tag. Use when releasing a new version.'
---

# Release Checklist

Walks through the steps to publish a new version of the FluentCards NuGet package.

## When to Use

- Publishing a new version to NuGet
- Preparing a release after feature work is complete

## Process

### Step 1: Update version

Version is defined using the GitVersion approach based on nbgv, to update the version follow https://dotnet.github.io/Nerdbank.GitVersioning/docs/nbgv-cli.html#preparing-a-release

### Step 2: Update CHANGELOG.md
<!-- TODO: Move [Unreleased] items under a new [X.Y.Z] - YYYY-MM-DD heading -->

### Step 3: Update PackageReleaseNotes
<!-- TODO: Update <PackageReleaseNotes> in FluentCards.csproj to match changelog -->

### Step 4: Verify
<!-- TODO: Run dotnet build && dotnet test && dotnet pack -->

### Step 5: Tag and push
<!-- TODO: git tag vX.Y.Z && git push origin vX.Y.Z (triggers CI publish) -->

## Constraints

- Follow semantic versioning.
- CI publish triggers on tags matching `v*`.

## Validation

- `dotnet pack` produces a valid .nupkg.
- CI build passes on the tagged commit.
