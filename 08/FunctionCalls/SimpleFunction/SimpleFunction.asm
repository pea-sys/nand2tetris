//SimpleFunction.test.2
//label SimpleFunction.test
(SimpleFunction.test)
//VMtranslator.Command.C_PUSH:constant:0
@0
D=A
@SP
A=M

M=D
@SP
M=M+1


//VMtranslator.Command.C_PUSH:constant:0
@0
D=A
@SP
A=M

M=D
@SP
M=M+1


//VMtranslator.Command.C_PUSH:local:0
@LCL
D=M
@0
A=D+A
D=M
@SP
A=M

M=D
@SP
M=M+1


//VMtranslator.Command.C_PUSH:local:1
@LCL
D=M
@1
A=D+A
D=M
@SP
A=M

M=D
@SP
M=M+1


//add
@SP
M=M-1

@SP
A=M

D=M
@SP
M=M-1

@SP
A=M

M=M+D
@SP
M=M+1

//not
@SP
M=M-1

@SP
A=M

@SP
A=M

M=!M
@SP
M=M+1

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


//add
@SP
M=M-1

@SP
A=M

D=M
@SP
M=M-1

@SP
A=M

M=M+D
@SP
M=M+1

//VMtranslator.Command.C_PUSH:argument:1
@ARG
D=M
@1
A=D+A
D=M
@SP
A=M

M=D
@SP
M=M+1


//sub
@SP
M=M-1

@SP
A=M

D=M
@SP
M=M-1

@SP
A=M

M=M-D
@SP
M=M+1

//Return
@LCL
D=M
@R13
M=D
@5
D=A
@R13
A=M-D
D=M
@R14
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
A=D-A
D=M
@THAT
M=D
@R13
D=M
@2
A=D-A
D=M
@THIS
M=D
@R13
D=M
@3
A=D-A
D=M
@ARG
M=D
@R13
D=M
@4
A=D-A
D=M
@LCL
M=D
@R14
A=M
0;JMP
