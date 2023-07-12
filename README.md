# Утилита сборки

Эмуляция системы сборки. Во входном файле указан список задач. Каждая задача определяется своим названием,
действиями и может зависеть от других задач. До начала выполнения задачи должны быть выполнены все ее зависимости.

Выполнение задачи состоит в запуске всех действий в виде команд интерпретатора командной строки.

# Входные данные
Файл с названием makefile в следующем формате:
```shell
target1: dependency1_1 dependency1_2 ... dependency1_N
 action1_1
 action1_2
 ...
 action1_M
...
targetX: dependencyX_1 dependencyX_2 ... dependencyX_R
 actionX_1
 ...
 actionX_T
 ```
 
где target1 ... targetX - названия задач, dependencyK_1 ... dependencyK_N - названия задач, от которых зависит задача K,
actionL_1...actionL_N - действия, выполняемые задачей L.

Названия задач указаны в начале строки, после чего следует двоеточие и список зависимостей задачи через один или несколько
пробелов. Действия указаны с отступом в один или больше пробелов или символов табуляции в начале строки.

# Запуск
Собранная версия приложения находится в директории publish.

Запускается из командной строки:
```shell
BuilderUtility 1.0.0
Copyright (C) 2023 BuilderUtility
  -p, --path      (Default: makefile.txt) Path to the file with tasks
  --help          Display this help screen.
  --version       Display version information.
  value pos. 0    Required. Executing task name
```
```shell
  Пример запуска: make.exe Target1
```

