# Building

Assuming you are running debian or alikes, first install dotnet repositories
```
$ sudo apt install dotnet dotnet-sdk-6.0 sqlite3
```
then clone this repository
```
$ git clone https://github.com/hjicks/gaweb
$ cd gaweb
$ dotnet build
$ dotnet tool restore 
$ dotnet ef migrations add v1
$ dotnet ef database update
$ ./MASsenger/bin/Debug/MASsenger
```

The main branch currently uses SQL Server, you may use `saeed-revisions` branch for SQLite based code.

# Design

Chat related classes:
```
BaseChat
|
|----> DirectChat
|
|           /----> ChannelChat
\----> BaseChan
            \----> GroupChat
```

User related classes:
```
BaseUser
|
|----> User
\----> Bot
```

Msg and related classes:
```
BaseMsg
|
|---> Msg
\---> FwdMsg
```
