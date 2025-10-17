# Building

Assuming you are running debian or alikes, first install dotnet repositories
```
$ sudo apt install dotnet
$ sudo apt install dotnet-sdk-6.0
```
then clone this repository
```
$ git clone https://github.com/hjicks/gaweb
$ cd gaweb
$ dotnet build
$ ./MASsenger/bin/Debug/MASsenger
```

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
