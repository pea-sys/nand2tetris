//VMtranslator.Command.C_PUSH
@3030
D=A
@SP
A=M
M=D

@SP
M=M+1

//VMtranslator.Command.C_POP
@R3
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
@3040
D=A
@SP
A=M
M=D

@SP
M=M+1

//VMtranslator.Command.C_POP
@R4
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
@32
D=A
@SP
A=M
M=D

@SP
M=M+1

//VMtranslator.Command.C_POP
@THIS
D=M
@2
D=D+A
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
@46
D=A
@SP
A=M
M=D

@SP
M=M+1

//VMtranslator.Command.C_POP
@THAT
D=M
@6
D=D+A
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
@R3
D=A
D=M
@SP
A=M
M=D

@SP
M=M+1

//VMtranslator.Command.C_PUSH
@R4
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

//VMtranslator.Command.C_PUSH
@THIS
D=M
@2
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
@THAT
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

