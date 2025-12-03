# Building

Assuming you are running debian or alikes, first install dotnet repositories:
```
$ sudo apt install dotnet dotnet-sdk-6.0 sqlite3
```
then clone this repository:
```
$ git clone https://github.com/mhs04/MessageEngine
$ cd gaweb
$ dotnet build
```


# Design

Chat related classes:
```
BaseChat
|
|----> PrivateChat
|                   
\----> ChannelChat
                    
```

User related classes:
```
BaseUser
|
|----> User
|
\----> Bot
```
