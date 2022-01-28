//VMtranslator.Command.C_PUSH
@10
D=A
@SP
A=M
M=D

@SP
M=M+1

//VMtranslator.Command.C_POP
@LCL
D=M
@0
A=D+A
@R13
M=D
@SP
M=M-1

@SP
A=M

D=M
@R13
A=M
M=D
//VMtranslator.Command.C_PUSH
@21
D=A
@SP
A=M
M=D

@SP
M=M+1

//VMtranslator.Command.C_PUSH
@22
D=A
@SP
A=M
M=D

@SP
M=M+1

//VMtranslator.Command.C_POP
@ARG
D=M
@2
A=D+A
@R13
M=D
@SP
M=M-1

@SP
A=M

D=M
@R13
A=M
M=D
//VMtranslator.Command.C_POP
@ARG
D=M
@1
A=D+A
@R13
M=D
@SP
M=M-1

@SP
A=M

D=M
@R13
A=M
M=D
//VMtranslator.Command.C_PUSH
@36
D=A
@SP
A=M
M=D

@SP
M=M+1

//VMtranslator.Command.C_POP
@THIS
D=M
@6
A=D+A
@R13
M=D
@SP
M=M-1

@SP
A=M

D=M
@R13
A=M
M=D
//VMtranslator.Command.C_PUSH
@42
D=A
@SP
A=M
M=D

@SP
M=M+1

//VMtranslator.Command.C_PUSH
@45
D=A
@SP
A=M
M=D

@SP
M=M+1

//VMtranslator.Command.C_POP
@THAT
D=M
@5
A=D+A
@R13
M=D
@SP
M=M-1

@SP
A=M

D=M
@R13
A=M
M=D
//VMtranslator.Command.C_POP
@THAT
D=M
@2
A=D+A
@R13
M=D
@SP
M=M-1

@SP
A=M

D=M
@R13
A=M
M=D
//VMtranslator.Command.C_PUSH
@510
D=A
@SP
A=M
M=D

@SP
M=M+1

//VMtranslator.Command.C_POP
@R11
D=A
@R13
M=D
@SP
M=M-1

@SP
A=M

D=M
@R13
A=M
M=D
//VMtranslator.Command.C_PUSH
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

//VMtranslator.Command.C_PUSH
@THAT
D=M
@5
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

//VMtranslator.Command.C_PUSH
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

//VMtranslator.Command.C_PUSH
@THIS
D=M
@6
A=D+A
D=M
@SP
A=M
M=D

@SP
M=M+1

//VMtranslator.Command.C_PUSH
@THIS
D=M
@6
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

//VMtranslator.Command.C_PUSH
@R11
D=A
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

