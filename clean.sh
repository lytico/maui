#!/bin/bash

#set -x

recursiverm() {
   printf 'clean '$1'\n'
   for f in $(find $1 -type d -iname "bin" -or -iname "obj" -or -iname '_resharper*' -or -iname '*.svn*'); do 
     printf $f'\n'
     rm -rf $f
   done
   find $1 -type f -iname "*.user" -or -iname "*.resharper" -or -iname '*.pidb' -or -iname '*.pdb' -delete
}


recursiverm "."

set +x
printf 'done\n'
sleep 5

