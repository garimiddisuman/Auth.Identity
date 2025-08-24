#!/bin/sh

dotnet build
rc=$?

if [[ $rc != 0 ]] ; then
    echo -e ""
    echo -e "❌ Cannot commit due to build failure"
    exit $rc
fi

dotnet test

rc=$?

if [[ $rc != 0 ]] ; then
    echo -e ""
    echo -e "❌ Cannot commit due to test failure"
    exit $rc
fi
