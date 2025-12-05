# Building

Assuming you are running debian or alikes, first install dotnet repositories:
```
$ sudo apt install dotnet dotnet-sdk-6.0 sqlite3
```
then clone this repository:
```
$ git clone https://github.com/hjicks/gaweb
$ cd gaweb
$ dotnet build
```


# Design
User related classes:
```
BaseUser
|
|----> User
|
\----> Bot

Session
```

Chat related classes:
```
BaseChat
|
|----> PrivateChat
|                   
\----> ChannelChat                    
```

Message related classes:
```
BaseMessage
|
|----> Message
|                   
\----> SystemMessage                    
```
