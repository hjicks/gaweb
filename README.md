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


# To Do:
1. Revision of BaseMessage entity ([@hjicks](https://github.com/hjicks))
2. Implement create and delete operations for BaseMessage entity using DTOs ([@hjicks](https://github.com/hjicks))
3. Implement create and delete operations for BaseChat entity using DTOs ([@Salimiyan](https://github.com/Salimiyan))
4. Add validation annotations for DTOs <ins>(gang of three)</ins>
5. Implement CQRS pattern with Mediatr package <ins>(gang of three)</ins>
