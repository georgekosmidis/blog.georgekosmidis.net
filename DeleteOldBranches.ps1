$cutoff = (Get-Date).AddDays(-90)
git for-each-ref --format='%(committerdate:iso8601)|%(refname:short)' refs/heads/ |
  ForEach-Object {
    $parts = $_ -split '\|'
    if ([datetime]$parts[0] -lt $cutoff -and $parts[1] -notmatch '^(main|master|develop)$') {
      git branch -D $parts[1]
    }
  }