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
```


# Design

Chat related classes:
```
BaseChat
|
|----> PrivateChat
|
|                   /----> ChannelChat
\----> ChannelGroupChat
                    \----> GroupChat
```

User related classes:
```
BaseUser
|
|----> User
\----> Bot
```
