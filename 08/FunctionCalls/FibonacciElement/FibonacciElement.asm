//Init
@256
D=A
@SP
M=D
//call Sys.init.0
@Sys.initRET0
D=A
@SP
A=M

M=D
@SP
M=M+1


@LCL
D=M
@SP
A=M

M=D
@SP
M=M+1


@ARG
D=M
@SP
A=M

M=D
@SP
M=M+1


@THIS
D=M
@SP
A=M

M=D
@SP
M=M+1


@THAT
D=M
@SP
A=M

M=D
@SP
M=M+1


@SP
D=M
@LCL
M=D
@SP
D=M
@5
D=D-A
@ARG
M=D
@Sys.init
0;JMP
(Sys.initRET0)
//Main.fibonacci.0
(Main.fibonacci)
//VMtranslator.Command.C_PUSH:argument:0
@ARG
D=M
@0
A=D+A
D=M
@SP
A=M

M=D
@SP
M=M+1


//VMtranslator.Command.C_PUSH:constant:2
@2
D=A
@SP
A=M

M=D
@SP
M=M+1


//lt
@SP
M=M-1

A=M
D=M

@SP
M=M-1

@SP
A=M

D=M-D
@BOOL_2
D;JLT
@SP
A=M

M=0
@ENDBOOL_2
0;JMP
(BOOL_2)
@SP
A=M

M=-1
(ENDBOOL_2)
@SP
M=M+1

//if IF_TRUE
@SP
M=M-1

A=M
D=M

@Main$IF_TRUE
D;JNE
//goto IF_FALSE
@Main$IF_FALSE
0;JMP
//label Main$IF_TRUE
(Main$IF_TRUE)
//VMtranslator.Command.C_PUSH:argument:0
@ARG
D=M
@0
A=D+A
D=M
@SP
A=M

M=D
@SP
M=M+1


//Return
@LCL
D=M
@R13
M=D
@SP
M=M-1

A=M
D=M

@ARG
A=M
M=D
@ARG
D=M
@SP
M=D+1
@R13
D=M
@1
D=D-A
A=D
D=M
@THAT
M=D
@R13
D=M
@2
D=D-A
A=D
D=M
@THIS
M=D
@R13
D=M
@3
D=D-A
A=D
D=M
@ARG
M=D
@R13
D=M
@4
D=D-A
A=D
D=M
@LCL
M=D
@R13
D=M
@5
D=D-A
A=D
D=M
@R14
M=D
@R14
A=M
0;JMP
//label Main$IF_FALSE
(Main$IF_FALSE)
//VMtranslator.Command.C_PUSH:argument:0
@ARG
D=M
@0
A=D+A
D=M
@SP
A=M

M=D
@SP
M=M+1


//VMtranslator.Command.C_PUSH:constant:2
@2
D=A
@SP
A=M

M=D
@SP
M=M+1


//sub
@SP
M=M-1

A=M
D=M

@SP
M=M-1

@SP
A=M

M=M-D
@SP
M=M+1

//call Main.fibonacci.1
@Main.fibonacciRET1
D=A
@SP
A=M

M=D
@SP
M=M+1


@LCL
D=M
@SP
A=M

M=D
@SP
M=M+1


@ARG
D=M
@SP
A=M

M=D
@SP
M=M+1


@THIS
D=M
@SP
A=M

M=D
@SP
M=M+1


@THAT
D=M
@SP
A=M

M=D
@SP
M=M+1


@SP
D=M
@LCL
M=D
@SP
D=M
@6
D=D-A
@ARG
M=D
@Main.fibonacci
0;JMP
(Main.fibonacciRET1)
//VMtranslator.Command.C_PUSH:argument:0
@ARG
D=M
@0
A=D+A
D=M
@SP
A=M

M=D
@SP
M=M+1


//VMtranslator.Command.C_PUSH:constant:1
@1
D=A
@SP
A=M

M=D
@SP
M=M+1


//sub
@SP
M=M-1

A=M
D=M

@SP
M=M-1

@SP
A=M

M=M-D
@SP
M=M+1

//call Main.fibonacci.1
@Main.fibonacciRET2
D=A
@SP
A=M

M=D
@SP
M=M+1


@LCL
D=M
@SP
A=M

M=D
@SP
M=M+1


@ARG
D=M
@SP
A=M

M=D
@SP
M=M+1


@THIS
D=M
@SP
A=M

M=D
@SP
M=M+1


@THAT
D=M
@SP
A=M

M=D
@SP
M=M+1


@SP
D=M
@LCL
M=D
@SP
D=M
@6
D=D-A
@ARG
M=D
@Main.fibonacci
0;JMP
(Main.fibonacciRET2)
//add
@SP
M=M-1

A=M
D=M

@SP
M=M-1

@SP
A=M

M=M+D
@SP
M=M+1

//Return
@LCL
D=M
@R13
M=D
@SP
M=M-1

A=M
D=M

@ARG
A=M
M=D
@ARG
D=M
@SP
M=D+1
@R13
D=M
@1
D=D-A
A=D
D=M
@THAT
M=D
@R13
D=M
@2
D=D-A
A=D
D=M
@THIS
M=D
@R13
D=M
@3
D=D-A
A=D
D=M
@ARG
M=D
@R13
D=M
@4
D=D-A
A=D
D=M
@LCL
M=D
@R13
D=M
@5
D=D-A
A=D
D=M
@R14
M=D
@R14
A=M
0;JMP
//Sys.init.0
(Sys.init)
//VMtranslator.Command.C_PUSH:constant:4
@4
D=A
@SP
A=M

M=D
@SP
M=M+1


//call Main.fibonacci.1
@Main.fibonacciRET3
D=A
@SP
A=M

M=D
@SP
M=M+1


@LCL
D=M
@SP
A=M

M=D
@SP
M=M+1


@ARG
D=M
@SP
A=M

M=D
@SP
M=M+1


@THIS
D=M
@SP
A=M

M=D
@SP
M=M+1


@THAT
D=M
@SP
A=M

M=D
@SP
M=M+1


@SP
D=M
@LCL
M=D
@SP
D=M
@6
D=D-A
@ARG
M=D
@Main.fibonacci
0;JMP
(Main.fibonacciRET3)
//label Sys$WHILE
(Sys$WHILE)
//goto WHILE
@Sys$WHILE
0;JMP
