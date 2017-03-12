# 4mtr
## Simple, opinionated text file formatter

```
$ 4mtr project1/ project2/ file1.txt file2.cs
```

Give a list of files or directories to 4mtr. For all text files found, 4mtr will:
- Ensure all lines end with the same character - CR, LF, CR/LF
- Ensure that there are not multiple empty lines at the end of a file
- Ensure that there is no whitespace at the end of a line

## Options
> `-e` or `--ending`

Specify a line ending character. If this is not specified, it swill use the system default.
Options are `n`, `r`, or `rn`.
Ex.
```
$ 4mtr -e n project1/
```

## Other
4mtr will currently ignore any files underneath directories with the names
- node_modules
- bower_components


### That's all, folks!