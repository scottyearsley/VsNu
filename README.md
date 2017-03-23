# VsNu

Identifies issues with NuGet references across multiple solutions in a repository.

## Issues identified

1. Assembly reference exists, but no `packages.config` entry.
2. Assembly reference does not exists, but no `packages.config` entry.
3. Assembly reference version does not match the `packages.config` version.
4. Assembly reference is not a NuGet sourced assembly

