$owner = "georgekosmidis"
$repo  = "blog.georgekosmidis.net"

# Get all remote branches except protected ones
$branches = git branch -r --merged origin/main |
  Where-Object { $_ -notmatch 'main|master|HEAD|develop' } |
  ForEach-Object { $_.Trim() -replace '^origin/','' }

# Delete in parallel (PowerShell 7+)
$branches | ForEach-Object -Parallel {
  gh api -X DELETE "repos/$using:owner/$using:repo/git/refs/heads/$_" 2>$null
  Write-Host "Deleted: $_"
} -ThrottleLimit 10

git fetch --all --prune

# Delete local branches whose upstream is gone
git branch -vv | Where-Object { $_ -match ': gone]' } | ForEach-Object {
  $branch = ($_ -split '\s+',3)[1]
  git branch -D $branch
}