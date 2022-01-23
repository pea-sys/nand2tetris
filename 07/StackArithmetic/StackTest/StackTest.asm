@17
D=A
@SP
A=M
M=D

@SP
M=M+1

@17
D=A
@SP
A=M
M=D

@SP
M=M+1

//eq
@SP
M=M-1

@SP
A=M

D=M
@SP
M=M-1

@SP
A=M

D=M-D
@TRUE_0
D;JEQ
@FALSE_0
0;JMP
(TRUE_0)
@SP
A=M

M=-1
@END_0
0;JMP
(FALSE_0)
@SP
A=M

M=0
(END_0)
@SP
M=M+1

@17
D=A
@SP
A=M
M=D

@SP
M=M+1

@16
D=A
@SP
A=M
M=D

@SP
M=M+1

//eq
@SP
M=M-1

@SP
A=M

D=M
@SP
M=M-1

@SP
A=M

D=M-D
@TRUE_1
D;JEQ
@FALSE_1
0;JMP
(TRUE_1)
@SP
A=M

M=-1
@END_1
0;JMP
(FALSE_1)
@SP
A=M

M=0
(END_1)
@SP
M=M+1

@16
D=A
@SP
A=M
M=D

@SP
M=M+1

@17
D=A
@SP
A=M
M=D

@SP
M=M+1

//eq
@SP
M=M-1

@SP
A=M

D=M
@SP
M=M-1

@SP
A=M

D=M-D
@TRUE_2
D;JEQ
@FALSE_2
0;JMP
(TRUE_2)
@SP
A=M

M=-1
@END_2
0;JMP
(FALSE_2)
@SP
A=M

M=0
(END_2)
@SP
M=M+1

@892
D=A
@SP
A=M
M=D

@SP
M=M+1

@891
D=A
@SP
A=M
M=D

@SP
M=M+1

//lt
@SP
M=M-1

@SP
A=M

D=M
@SP
M=M-1

@SP
A=M

D=M-D
@TRUE_3
D;JLT
@FALSE_3
0;JMP
(TRUE_3)
@SP
A=M

M=-1
@END_3
0;JMP
(FALSE_3)
@SP
A=M

M=0
(END_3)
@SP
M=M+1

@891
D=A
@SP
A=M
M=D

@SP
M=M+1

@892
D=A
@SP
A=M
M=D

@SP
M=M+1

//lt
@SP
M=M-1

@SP
A=M

D=M
@SP
M=M-1

@SP
A=M

D=M-D
@TRUE_4
D;JLT
@FALSE_4
0;JMP
(TRUE_4)
@SP
A=M

M=-1
@END_4
0;JMP
(FALSE_4)
@SP
A=M

M=0
(END_4)
@SP
M=M+1

@891
D=A
@SP
A=M
M=D

@SP
M=M+1

@891
D=A
@SP
A=M
M=D

@SP
M=M+1

//lt
@SP
M=M-1

@SP
A=M

D=M
@SP
M=M-1

@SP
A=M

D=M-D
@TRUE_5
D;JLT
@FALSE_5
0;JMP
(TRUE_5)
@SP
A=M

M=-1
@END_5
0;JMP
(FALSE_5)
@SP
A=M

M=0
(END_5)
@SP
M=M+1

@32767
D=A
@SP
A=M
M=D

@SP
M=M+1

@32766
D=A
@SP
A=M
M=D

@SP
M=M+1

//gt
@SP
M=M-1

@SP
A=M

D=M
@SP
M=M-1

@SP
A=M

D=M-D
@TRUE_6
D;JGT
@FALSE_6
0;JMP
(TRUE_6)
@SP
A=M

M=-1
@END_6
0;JMP
(FALSE_6)
@SP
A=M

M=0
(END_6)
@SP
M=M+1

@32766
D=A
@SP
A=M
M=D

@SP
M=M+1

@32767
D=A
@SP
A=M
M=D

@SP
M=M+1

//gt
@SP
M=M-1

@SP
A=M

D=M
@SP
M=M-1

@SP
A=M

D=M-D
@TRUE_7
D;JGT
@FALSE_7
0;JMP
(TRUE_7)
@SP
A=M

M=-1
@END_7
0;JMP
(FALSE_7)
@SP
A=M

M=0
(END_7)
@SP
M=M+1

@32766
D=A
@SP
A=M
M=D

@SP
M=M+1

@32766
D=A
@SP
A=M
M=D

@SP
M=M+1

//gt
@SP
M=M-1

@SP
A=M

D=M
@SP
M=M-1

@SP
A=M

D=M-D
@TRUE_8
D;JGT
@FALSE_8
0;JMP
(TRUE_8)
@SP
A=M

M=-1
@END_8
0;JMP
(FALSE_8)
@SP
A=M

M=0
(END_8)
@SP
M=M+1

@57
D=A
@SP
A=M
M=D

@SP
M=M+1

@31
D=A
@SP
A=M
M=D

@SP
M=M+1

@53
D=A
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

@112
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

//neg
@SP
M=M-1

@SP
A=M

@SP
A=M

M=-M
@SP
M=M+1

//and
@SP
M=M-1

@SP
A=M

D=M
@SP
M=M-1

@SP
A=M

M=M&D
@SP
M=M+1

@82
D=A
@SP
A=M
M=D

@SP
M=M+1

//or
@SP
M=M-1

@SP
A=M

D=M
@SP
M=M-1

@SP
A=M

M=M|D
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

