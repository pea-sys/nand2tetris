//VMtranslator.Command.C_PUSH:constant:0
@0
D=A
@SP
A=M
M=D

@SP
M=M+1

//VMtranslator.Command.C_POP:local:0
@LCL
D=M
@0
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
($BasicLoop.vm.LOOP_START)
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

//VMtranslator.Command.C_POP:local:0
@LCL
D=M
@0
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

//VMtranslator.Command.C_POP:argument:0
@ARG
D=M
@0
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

@SP
M=M-1

@SP
A=M

D=M
@$BasicLoop.vm.LOOP_START
D;JNE
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

