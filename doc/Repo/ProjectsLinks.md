# Схема зависимостей проектов 

```mermaid
flowchart TD
    common[Sphagnum.Common]
    unitTestsCommon[Sphagnum.Common.UnitTests]
    client[Sphagnum.Client]
    server[Sphagnum.Server]

server --> common
client ---> common
unitTestsCommon ---> common
```
