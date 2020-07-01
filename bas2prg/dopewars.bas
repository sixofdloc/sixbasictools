'--------------------------------------------------
'- Dopewars 
'- Commodore 64
'- By Six/Style 2015
'- Based on "Drug Wars" by John E. Dell
'--------------------------------------------------
'-Still to do - highscore saving, jail, dropped things
'
'
'--------------------------------------------------
'-----Initialization
'--------------------------------------------------
1 GOSUB 60000 
10 POKE 53280,11:POKE 53281,0:PRINT "{clear}{white}{del}"; 
11 POKE 53272,23: GOSUB 500 
12 CN=0:GOSUB 2000 
'------------------------------------------------
'------Main Menu
'--------------------------------------------------
100 GOSUB 1000:GOSUB 3030:GOSUB 3000 
102 IF A$ <> "t" AND A$<>"T" THEN GOTO 110 
104 GOSUB 1300: GOTO 100 
110 IF A$ <> "b" AND A$<>"B" THEN GOTO 120 
111 GOSUB 1100: GOTO 100 
120 IF A$ <> "s" AND A$<>"S" THEN GOTO 100 
122 GOSUB 1200: GOTO 100 
124 END  
'------------------------------------------------
'------Intro
'--------------------------------------------------
500 PRINT "{clear}{red}             DopeWars v4.20{white}" 
510 PRINT "          {yellow}By {white}Six of Style{yellow}, 2015":PRINT  
520 PRINT "{$0d}  {orange}Based on 'Drug Wars' By John E. Dell" :PRINT  
521 PRINT "{blue}This is a game of buying, selling, and" 
522 PRINT "{light blue}fighting. The object of the game is to" 
523 PRINT "{cyan}pay off your debt to the loan shark, " 
524 PRINT "{light green}then, make as much money as you can " 
525 PRINT "{white}in 90 days. If you deal too heavily" 
526 PRINT "{light green}in drugs,  you  might  run  into  the" 
527 PRINT "{cyan}police!!  Your main drug stash will be" 
528 PRINT "{light blue}in the Bronx. (fta was here" 
529 PRINT "{blue}(It's a nice neighborhood)!":PRINT :PRINT  
530 GOSUB 3020 
535 X = RND(-TI) 
540 RETURN 
'------------------------------------------------
'------Print Divider
'--------------------------------------------------
600 PRINT "````````````````````````````````````````":RETURN 
'------------------------------------------------
'------Main Display
'--------------------------------------------------
1000 PRINT "{white}{clear}"; 
1005 PRINT "{$b0}``````````````````````````````````````{$ae}"; 
1008 PRINT "}    Dopewars v4.20 by Six of Style    }"; 
1010 PRINT "{$ab}``````````````````````````````````````{$b3}"; 
1011 PRINT "}{yellow}Current Location{white}:                     {white}}"; 
1012 PRINT "}{yellow}Cash{white}:{yellow}              Debt{white}:{yellow}              {white}}"; 
1013 PRINT "}{yellow}Bank{white}:{yellow}              Health{white}:{yellow}            {white}}"; 
1014 PRINT "}{yellow}Bitches{white}:{yellow}      Guns{white}:{yellow}      Carry{white}:       }"; 
1015 PRINT "{$ab}```````````````{$b2}``````````````````````{$b3}"; 
1016 PRINT "}{reverse on}{blue}Selling {black} {blue}Price {reverse off}{white}}{reverse on}{blue}Carrying {black} {blue}Paid  {black} {blue}Qty  {reverse off}{white}}"; 
1017 PRINT "{$ab}```````````````{``````````````````````{$b3}"; 
1018 FOR I = 1 TO 12:PRINT "}               }                      }";:NEXT I 
1019 PRINT "{$ab}```````````````{$b1}``````````````````````{$b3}"; 
1020 PRINT "}{red}{reverse on}B{reverse off}uy, {reverse on}S{reverse off}ell, {reverse on}T{reverse off}ravel{white}:                    }"; 
1021 PRINT "{$ad}`````````````````````````````````````{$bd}{left}{left}{insert}`{home}"; 
1028 X=19:Y=3:GOSUB 3050:PRINT "{red}";LO$(CN); 
1032 Y=4:X=6:GOSUB 3050:PRINT "{green}";C; 
1034 Y=4:X=25:GOSUB 3050:PRINT D; 
1035 Y=5:X=6:GOSUB 3050:PRINT BK; 
1036 Y=6:X=20:GOSUB 3050:PRINT G; 
1037 Y=6:X=9:GOSUB 3050:PRINT NB; 
1038 Y=5:X=27:GOSUB 3050: PRINT H;"%"; 
1048 GOSUB 5100:Y=6:X=32:GOSUB 3050:PRINT CL; 
1079 Y=9:FOR I = 0 TO ND-1 
1080 IF HD(I)=0 THEN GOTO 1083 
1081 Y=Y+1:X=1:GOSUB 3050 
1082 PRINT "{purple}";DN$(I);:X=9:GOSUB 3050:PRINT "{green}";INT(DP(I)) 
1083 NEXT I 
1084 Y=9:FOR I = 0 TO ND-1 
1085 IF DC(I)=0 THEN GOTO 1089 
1086 Y=Y+1:X=17:GOSUB 3050 
1087 PRINT "{cyan}";DN$(I);:X=27:GOSUB 3050:PRINT "{green}";AP(I); 
1088 X=33:GOSUB 3050:PRINT "{yellow}";DC(I); 
1089 NEXT I 
1098 PRINT "{white}";:Y=23:X=20:GOSUB 3050 
1099 RETURN 
'------------------------------------------------
'------Buy Drugs
'--------------------------------------------------
1100 PRINT "Buy Drugs":PRINT :PRINT  
1102 GOSUB 1400 
1105 PRINT :PRINT "{white}Buy what? {yellow}"; 
1110 GOSUB 3010 
1115 IF VAL(B$)=0 THEN GOTO 58001 
1120 WD=99:FOR I = 0 TO ND-1 
1122 IF VAL(B$) = PN(I) THEN WD=I 
1124 NEXT I 
1126 IF WD=99 THEN GOTO 58001 
1128 IF HD(WD)=0 THEN GOTO 1146 
1129 PRINT :PRINT "You can afford ";INT(C/DP(WD));" units." 
1130 PRINT "Buy how many? {yellow}";:GOSUB 3010:IF VAL(B$)=0 THEN RETURN 
1134 BD = VAL(B$) 
1136 IF BD*DP(WD)>C THEN GOTO 58000 
1138 GOSUB 5100:IF BD>CL THEN GOTO 58002 
1139 IF DC(WD) > 0 THEN GOTO 1142 
1140 AP(WD)=DP(WD): GOTO 1142 
1141 AP(WD) = INT(((AP(WD)*DC(WD))+(BD*DP(WD)))/(DC(WD)+BD)) 
1142 C=C-(BD*DP(WD)):DC(WD)=DC(WD)+BD 
1144 PRINT :PRINT "{cyan}Purchased "+STR$(BD)+" units of ";DN$(WD) 
1145 GOSUB 5000:RETURN 
1146 PRINT :PRINT "{red}That's not for sale here!":GOSUB 5000:RETURN 
1199 RETURN 
'------------------------------------------------
'-----Sell Drugs
'--------------------------------------------------
1200 PRINT "Sell Drugs":PRINT :PRINT  
1205 FOR I = 0 TO ND-1:IF DC(I)>0 THEN GOTO 1210 
1206 NEXT I 
1208 PRINT "{red}You don't have shit to sell!":GOSUB 5000:RETURN 
1210 GOSUB 1500:PRINT :PRINT "{white}Sell what? {yellow}";:GOSUB 3010 
1212 IF VAL(B$)=0 THEN GOTO 58001 
1220 J=99:FOR I = 0 TO ND-1 
1222 IF VAL(B$)=PI(I) THEN GOTO 1225 
1223 NEXT I 
1224 GOTO 1226 
1225 J=I 
1226 IF J=99 THEN GOTO 58001 
1227 IF HD(J)=0 THEN GOTO 1281 
1230 PRINT :PRINT "{white}How many units? {yellow}";:GOSUB 3010:IF VAL(B$)=0 THEN GOTO 58001 
1235 I=VAL(B$):IF I>DC(J) THEN GOTO 58003 
1240 C=C+(I*DP(J)):DC(J)=DC(J)-I 
1242 PRINT :PRINT "{cyan}Sold "+STR$(I)+" units of ";DN$(J):GOSUB 5000 
1245 RETURN 
1281 PRINT :PRINT "{red}No buyers for that here!":GOSUB 5000:RETURN 
1299 RETURN 
'------------------------------------------------
'------Travel
'--------------------------------------------------
1300 PRINT "Travel":PRINT :PRINT  
1305 GOSUB 4000 
1310 PRINT :PRINT "{white}Destination: {yellow}";:GOSUB 3030:GOSUB 3000 
1315 IF VAL(A$)<1 OR VAL(A$)>NN THEN GOTO 58001 
1320 CN = VAL(A$)-1:GOSUB 2000:RETURN 
'------------------------------------------------
'------List Drugs Available in Current Location
'--------------------------------------------------
1400 FOR I = 0 TO ND-1:IF HD(I)=0 THEN GOTO 1410 
1405 PRINT "{white}";PN(I);". {blue}";DN$(I);SPC4);TAB(16);"{green}$";INT(DP(I)) 
1410 NEXT I 
1499 RETURN 
'------------------------------------------------
'------List Current Inventory
'--------------------------------------------------
1500 J=1:FOR I = 0 TO ND-1:PI(I)=99:NEXT I 
1502 FOR I = 0 TO ND-1:IF DC(I)=0 THEN GOTO 1515 
1510 PRINT "{white}";J". {blue}";DN$(I);"  {green}";STR$(DC(I)) 
1512 PI(I)=J:J=J+1 
1515 NEXT I 
1599 RETURN 
'------------------------------------------------
'------Travelling To Area - Initialize Drugs In Location
'--------------------------------------------------
2000 PRINT :PRINT "{white}Travelling to: {red}";LO$(CN) 
2003 J = 1 
2005 FOR I=0 TO ND-1:K=RND(1)*100:HD(I)=0 
2008 IF K<DR(I) THEN HD(I)=1 
2010 IF HD(I)=1 THEN GOTO 2012 
2011 PN(I)=99: GOTO 2020 
2012 DP(I) = INT(LP(I)+((HP(I)-LP(I))*RND(1))) 
2013 PN(I) = J:J = J + 1 
2020 NEXT I 
'------------------------------------------------
'---Do Interest on Debt
'------------------------------------------------
2025 IF D > 0 THEN D = INT(D*1.05) 
'------------------------------------------------
'------Enter Area - Expensive Events?
'--------------------------------------------------
2030 J=0:FOR I = 0 TO ND-1 
2031 IF HD(I)=0 THEN GOTO 2038 
2032 IF CE(I)=0 THEN GOTO 2038 
2033 IF J>3 THEN GOTO 2038 
2034 IF RND(1)*100<50 THEN GOTO 2038 
2035 J=J+1:DP(I)=DP(I)*4 
2036 GOSUB 2900 
2038 NEXT I 
'------------------------------------------------
'------Enter Area - Cheap Events?
'--------------------------------------------------
2040 FOR I = 0 TO ND-1 
2041 IF HD(I)=0 THEN GOTO 2048 
2042 IF CC(I)=0 THEN GOTO 2048 
2043 IF J>3 THEN GOTO 2048 
2044 IF RND(1)*100<50 THEN GOTO 2048 
2045 J=J+1:DP(I)=INT(DP(I)/4) 
2046 GOSUB 2950 
2048 NEXT I 
'--------------------------------------------------
'------Enter Area - Lady On Subway Event?
'--------------------------------------------------
2050 IF RND(1)*100>70 THEN GOTO 2060 
2051 PRINT :PRINT "{yellow}A lady on the subway says...{orange}":PRINT LS$(INT(RND(1)*NL)) 
2052 PRINT  
'--------------------------------------------------
'------Enter Area - Stopped To Event?
'--------------------------------------------------
2060 IF RND(1)*100>20 THEN GOTO 2100 
2061 PRINT :PRINT "{purple}You stopped to ";ST$(INT(RND(1)*NS)) 
2062 PRINT  
'--------------------------------------------------
'------Enter Area - Loan Shark Attack?
'--------------------------------------------------
2100 REM  
2130 IF D<10000 THEN GOTO 2190 
2132 IF D>15000 THEN GOTO 2136 
2134 PRINT "{red}The Loan Shark says you'd better{$0d}pay up soon - or else!{$0d}" 
2135 GOTO 2190 
2136 IF D>30000 THEN GOTO 2190 
2138 PRINT "{red}The Loan Shark sent some guys{$0d}around to kick your ass.{$0d}" 
2139 REM IF B >10 THEN GOTO 2142 
2140 H = H-INT(RND(1)*10):IF H<=0 THEN GOTO 59000 
'--------------------------------------------------
'------Enter Area - Loan Shark Event?
'--------------------------------------------------
2190 IF CN <> 0 THEN GOTO 2200 
2191 PRINT "{white}Visit the Loan Shark?(Y/N):{yellow}";:GOSUB 5200 
2192 IF YN=0 THEN GOTO 2200 
2195 GOSUB 5300 
'--------------------------------------------------
'------Enter Area - Hire Opportunity Event?
'--------------------------------------------------
2200 IF INT(RND(1)*100)>BO(CN) THEN GOTO 2250 
2201 GOSUB 5500 
'--------------------------------------------------
'------Enter Area - Dropped Drugs Event?
'--------------------------------------------------
2250 REM  
'--------------------------------------------------
'------Enter Area - Found Drugs Event?
'--------------------------------------------------
2300 IF CL <=0 THEN GOTO 2350 
2302 IF INT(RND(1)*100>5) THEN GOTO 2350 
2303 I = INT(RND(1)*(ND-1)):J=INT(RND(1)*10)+2 
2305 IF J > CL THEN J = CL 
2310 PRINT :PRINT "{purple}You found ";J;" units of ";DN$(I);"{$0d} on the subway!" 
2311 IF DC(I)=0 THEN AP(I)=0 
2315 AP(I)= INT(((DC(I))*AP(I))/(DC(I)+J)) 
2319 DC(I)=DC(I)+J:GOSUB 5100 
'--------------------------------------------------
'------Enter Area - Paraquat Event?
'--------------------------------------------------
2350 IF INT(RND(1)*100)>5 THEN GOTO 2400 
2352 PRINT :PRINT "{light green}There is some weed here that{$0d}smells like paraquat." 
2354 PRINT "Smoke it?{white}(Y/N)";:GOSUB 5200:IF YN=0 THEN GOTO 2390 
2364 IF INT(RND(1)*100)<30 THEN GOTO 2370 
2366 PRINT "{red}It made you sick as hell.":H=H-10:IF H<=0 THEN GOTO 59000 
2367 GOSUB 5100: GOTO 2400 
2370 J = INT(RND(1)*CL): PRINT :PRINT "{orange}It was great!" 
2372 IF J <=0 THEN GOTO 2400 
2374 PRINT "The dealer comps you ";J;" units of it." 
2375 IF DC(11)=0 THEN AP(11)=0 
2377 AP(11)=INT((DC(11)*AP(11))/(DC(11)+J)) 
2379 DC(11) = DC(11)+J: GOTO 2400:GOSUB 5100 
2390 PRINT :PRINT "{white}Probably the wisest move." 
'--------------------------------------------------
'------Enter Area - Fight Event?
'--------------------------------------------------
2400 IF INT(RND(1)*100)>PP(CN) THEN GOTO 2410 
2401 GOSUB 6000: GOTO 2450 
2410 REM  
'-------------------------------------------------
'------Enter Area - Pub Event?
'--------------------------------------------------
2450 IF CN <> 1 THEN GOTO 2500 
2452 PRINT :PRINT "{white}Stop at the bar?(Y/N){yellow}";:GOSUB 5200 
2454 IF YN=0 THEN GOTO 2500 
2456 GOSUB 5400 
'--------------------------------------------------
'------Enter Area - Gun Store Event?
'--------------------------------------------------
2500 IF CN <> 5 THEN GOTO 2550 
2501 PRINT :PRINT "{white}Visit the gun store?(Y/N){yellow}";:GOSUB 5200 
2502 IF YN=0 THEN GOTO 2550 
2503 GOSUB 5600 
'--------------------------------------------------
'------Enter Area - Bank Event?
'--------------------------------------------------
2550 IF CN <> 3 THEN GOTO 2599 
2552 PRINT :PRINT "{white}Visit the bank?(Y/N){yellow}";:GOSUB 5200 
2554 IF YN=0 THEN GOTO 2599 
2556 GOSUB 5800 
2599 REM  
'--------------------------------------------------
'-----Hospital?
'--------------------------------------------------
2600 IF CN <> 7 THEN GOTO 2870 
2602 PRINT :PRINT "{white}Visit the hospital?(Y/N){yellow}";:GOSUB 5200 
2604 IF YN=0 THEN GOTO 2870 
2606 GOSUB 5700 
'--------------------------------------------------
'------Enter Area - Music Playing Event?
'--------------------------------------------------
2870 IF RND(1)*100>15 THEN GOTO 2887 
2871 PRINT :PRINT "{cyan}You hear music playing...{$0d}{yellow}It's "; 
2872 PRINT MP$(INT(RND(1)*NM)):PRINT  
'--------------------------------------------------
'----Travel round is over
'--------------------------------------------------
2887 PRINT "{white}":PRINT :PRINT  
2890 GOSUB 3020 
2891 DA = DA + 1:IF DA=91 THEN GOTO 57000 
2899 RETURN 
'--------------------------------------------------
'------Print Drug Expensive Message (Expects drug in I)
'--------------------------------------------------
2900 IF RND(1)*100>50 THEN GOTO 2910 
2905 PRINT :PRINT E1$;DN$(I);E2$::PRINT :GOSUB 5000 
2907 RETURN 
2910 PRINT :PRINT E3$;DN$(I);E4$::PRINT :GOSUB 5000 
2920 RETURN 
'--------------------------------------------------
'------Print Drug Cheap Message (Expects drug in I)
'--------------------------------------------------
2950 PRINT :PRINT CD$(I):PRINT :GOSUB 5000 
2999 RETURN 
'------------------------------------------------
'------Get Single Char
'--------------------------------------------------
3000 GET A$:IF A$="" THEN GOTO 3000 
3002 RETURN 
'------------------------------------------------
'------Get String
'--------------------------------------------------
3010 B$ = "":GOSUB 3030 
3012 GOSUB 3060 
3013 IF A$="{$0d}" THEN RETURN 
3014 IF A$<>"{del}" THEN GOTO 3018 
3015 IF LEN(B$)=0 THEN GOTO 3012 
3016 B$=LEFT$(B$,LEN(B$)-1):PRINT A$;: GOTO 3012 
3018 B$ = B$ + A$:PRINT A$;: GOTO 3012 
'------------------------------------------------
'------Press Any Key
'--------------------------------------------------
3020 GOSUB 3030:PRINT :PRINT "{white}       Press Any Key To Continue" 
3022 GOSUB 3000 
3023 RETURN 
'------------------------------------------------
'------Clear Keyboard Buffer
'------------------------------------------------
3030 GET A$:IF A$<>"" THEN GOTO 3030 
3031 RETURN 
'------------------------------------------------
'------Position CUrsor (Expects X, Y preset)
'--------------------------------------------------
3050 POKE 781,Y:POKE 782,X:POKE 783,0:SYS 65520 
3055 RETURN 
'-----------------------------------------------
'---Get numeric char into a$
'-----------------------------------------------
3060 C$ = "0123456789{del}{$0d}" 
3061 GET A$:IF A$="" THEN GOTO 3061 
3062 FOR I = 1 TO LEN(C$):IF MID$(C$,I,1)=A$ THEN RETURN 
3063 NEXT  
3064 GOTO 3061 
'------------------------------------------------
'------List Locations
'--------------------------------------------------
4000 FOR I = 0 TO NN-1 
4002 IF I = CN THEN GOTO 4007 
4005 PRINT "{white}";I+1;". {blue}";LO$(I) 
4006 GOTO 4010 
4007 PRINT "{white}";I+1;". {blue}";LO$(I);"{white}<-Current Location" 
4010 NEXT I 
4099 RETURN 
'------------------------------------------------
'----List Guns
'------------------------------------------------
4100 FOR I = 0 TO NG-1 
4102 PRINT "{white}";I+1;". {blue}";GN$(I);"    {green}$";GP(I) 
4104 NEXT I 
4106 RETURN 
'------------------------------------------------
'----List Player's Guns
'------------------------------------------------
4150 FOR I = 0 TO NG-1 
4152 PRINT "{white}";I+1;". {blue}";GN$(I);"    {yellow}";GC(I) 
4154 NEXT I 
4156 RETURN 
'------------------------------------------------
'------Delay
'--------------------------------------------------
5000 FOR I = 0 TO 1000:NEXT I:RETURN 
'------------------------------------------------
'------Calculate Remaining Carrying Capacity
'------Drop guns drugs when needed
'------------------------------------------------
'--Carry is health + (bitches * 10)-guns-drugs
5100 GOSUB 5180:GOSUB 7000: CL=H+(NB*10)-G 
5102 FOR I = 0 TO ND-1 
5104 IF DC(I)>0 THEN CL = CL - DC(I) 
5105 NEXT I 
5106 IF CL >= 0 THEN RETURN 
5108 QL = ABS(CL) 
5110 FOR I = 0 TO ND-1 
5111 IF DC(I)= 0 THEN GOTO 5117 
5112 IF DC(I)>=QL THEN GOTO 5118 
5114 QL=QL-DC(I):DC(I)=0:IF QL=0 THEN GOTO 5119 
5117 NEXT I 
5118 DC(I)=DC(I)-QL 
5119 CL=0 
5120 RETURN 
'------------------------------------------------
'---Update G
'------------------------------------------------
5180 G=0:FOR I = 0 TO NG-1:G=G+GC(I):NEXT :RETURN 
'------------------------------------------------
'-- Yes/No Response
'------------------------------------------------
5200 YN=0:GOSUB 3030:GOSUB 3000 
5202 IF A$<>"Y" AND A$<>"y" THEN  GOTO 5210 
5205 YN=1:PRINT "Yes.":PRINT :RETURN 
5210 YN=0:PRINT "No.":PRINT :RETURN 
'------------------------------------------------
'-- Loan Shark
'------------------------------------------------
5300 PRINT  
5301 PRINT "{white}You enter the loan shark's office.{$0d}It smells of sweat and garlic." 
5302 PRINT :PRINT "{white}You owe {green}$";D;"{white}." 
5303 PRINT "You have {green}$";C;"{white}." 
5306 PRINT "Pay {reverse on}S{reverse off}ome, Pay {reverse on}A{reverse off}ll, {reverse on}L{reverse off}eave: {yellow}"; 
5310 GOSUB 3030:GOSUB 3000 
5312 IF A$<>"s" AND A$<>"S" THEN GOTO 5330 
5314 PRINT "Some.{$0d}{white}How much? {yellow}";:GOSUB 3010:PS=VAL(B$) 
5316 IF PS=0 THEN GOTO 5302 
5318 IF PS<=D THEN GOTO 5322 
5320 PRINT :PRINT "{red}You don't owe that much.": GOTO 5302 
5322 D=D-PS: IF D = 0 THEN RETURN 
5324 GOTO 5302 
5330 IF A$<>"a" AND A$<>"A" THEN GOTO 5350 
5331 PRINT "All." 
5332 IF D <= C THEN GOTO 5345 
5334 PRINT :PRINT "{red}You don't have that much.": GOTO 5302 
5345 C=C-D:D=0:PRINT  
5346 PRINT "{orange}The Loan Shark thanks you for{$0d}your business.":PRINT  
5348 RETURN 
5350 IF A$<>"l" AND A$<>"L" THEN GOTO 5302 
5352 PRINT "Leave":RETURN 
'------------------------------------------------
'-----Bar
'------------------------------------------------
5400 PRINT "{white}The neighborhood bar is dark and smoky." 
5401 PRINT "You find a seat in the corner." 
5406 GOSUB 5500 
5408 PRINT :PRINT "{white}Cash: {green}$";C;"{white}, Health: {yellow}";H;"%" 
5410 PRINT :PRINT "{white}{reverse on}D{reverse off}rink, {reverse on}C{reverse off}urse, {reverse on}G{reverse off}amble, {reverse on}L{reverse off}eave: {yellow}"; 
5412 GOSUB 3030:GOSUB 3000 
5414 IF A$<>"D" AND A$<>"d" THEN GOTO 5425 
5416 IF C < 2 THEN GOTO 5429 
5418 C = C - 2:H=H+2:IF H > 100 THEN H = 100 
5419 PRINT "Drink" 
5420 PRINT :PRINT "{orange}You have a beer.  You feel better.":GOSUB 5000: GOTO 5408 
5425 IF A$<>"C" AND A$<>"c" THEN GOTO 5445 
5426 PRINT "Curse" 
5427 J=INT(RND(1)*(NW-1)):PRINT :PRINT "{red}You yell '";C0$(J);" "; 
5428 J=INT(RND(1)*(NW-1)):PRINT C0$(J);" "; 
5429 J=INT(RND(1)*(NW-1)):PRINT C1$(J);" "; 
5430 J=INT(RND(1)*(NW-1)):PRINT C0$(J);","; 
5431 J=INT(RND(1)*(NW-1)):PRINT C1$(J);"!"; 
5432 J=INT(RND(1)*100): IF J < 80 THEN GOTO 5444 
5436 IF J < 97 THEN GOTO 5440 
5437 PRINT :PRINT "{orange}Everyone laughs.  Someone hands you $100!" 
5438 C = C + 100:GOSUB 5000 : GOTO 5408 
5440 PRINT :PRINT "{orange}Someone throws a bottle at you.":H=H-5:GOSUB 5000 
5441 IF H <= 0 THEN GOTO 59000 
5442 GOTO 5408 
5444 PRINT :PRINT "{orange}No one is impressed.":GOSUB 5000: GOTO 5408 
5445 IF A$<>"G" AND A$<>"g" THEN GOTO 5474 
5447 PRINT "Gamble{$0d}{$0d}{white}Bet Amount? {yellow}";:GOSUB 3010 
5449 IF VAL(B$)>C THEN GOTO 5473 
5450 ZI = VAL(B$):PRINT :PRINT "{white} Bet on (3-12, not 7 or 11): {yellow}";:GOSUB 3010 
5460 ZJ = VAL(B$): IF ZJ<2 OR ZJ>12 OR ZJ=7 OR ZJ=11 THEN GOTO 5472 
5462 K0=INT(RND(1)*6):K1=INT(RND(1)*6):PRINT :PRINT "{white}You roll the dice..." 
5463 PRINT K0;" and ";K1; 
5466 IF K0+K1 = 2 THEN PRINT " - snake eyes." 
5468 IF K0+K1 = 7 OR K0+K1=11 THEN PRINT " - house wins on ";K0+K1;"." 
5469 IF K0+K1<> ZJ THEN GOTO 5471 
5470 C=C+(ZI*(INT(RND(1)*10)+1)):PRINT :PRINT "{orange}You won!":GOSUB 5000: GOTO 5408 
5471 C=C-ZI:PRINT :PRINT "{red}You lost.":GOSUB 5000: GOTO 5408 
5472 GOSUB 58001: GOTO 5408 
5473 GOSUB 58004: GOTO 5408 
5474 IF A$<>"L" AND A$<>"l" THEN GOTO 5408 
5475 PRINT "Leave{$0d}":PRINT :PRINT "{orange}You stumble out of the bar.":GOSUB 5000:PRINT :RETURN 
'------------------------------------------------
'-----Hiring Event
'------------------------------------------------
5500 J = INT(RND(1)*100000) 
5505 PRINT :PRINT "{white}Hire another bitch for {green}$";J;"{white}? (Y/N){yellow}"; 
5510 GOSUB 5200:IF YN=0 THEN GOTO 5525 
5512 IF J<=C THEN GOTO 5520 
5515 PRINT :PRINT "{red}You can't afford that much!": GOTO 5525 
5520 C = C - J:NB=NB+1:GOSUB 5100:PRINT "{orange}You now have ";NB;" bitches." 
5525 RETURN 
'------------------------------------------------
'----Gun Store
'------------------------------------------------
5600 PRINT :PRINT "{white}You enter the gun store.{$0d}On the far wall, there's a giant poster" 
5601 PRINT "of Ted Nugent wearing a flag and {$0d}fellating an eagle." 
5602 REM  
5604 PRINT "You have {green}$";C;"{white}." 
5606 PRINT "{reverse on}B{reverse off}uy, {reverse on}S{reverse off}ell, S{reverse on}h{reverse off}oplift, {reverse on}L{reverse off}eave: {yellow}"; 
5610 GOSUB 3030:GOSUB 3000 
5612 IF A$<>"b" AND A$<>"B" THEN GOTO 5630 
5614 PRINT "Buy{$0d}":GOSUB 4100:PRINT "Buy what? {yellow}";:GOSUB 3010 
5615 VJ=VAL(B$)-1:IF VJ<0 OR VJ>NG-1 THEN GOTO 5647 
5616 PRINT :PRINT "{white}How many?{yellow} ";:GOSUB 3010:VI=VAL(B$):IF I=0 THEN GOTO 5647 
5617 IF (VI*GP(VJ))>C THEN GOTO 5649 
5618 C=C-(VI*GP(VJ)):GC(VJ)=GC(VJ)+VI 
5619 PRINT :PRINT "{cyan}Purchased "+STR$(VI)+" ";GN$(VJ)+"s":GOSUB 5180: GOSUB 5000 
5629 GOTO 5602 
5630 IF A$<>"S" AND A$<>"s" THEN GOTO 5650 
5631 PRINT "Sell{$0d}":GOSUB 7100:IF J = 0 THEN GOTO 5648 
5632 GOSUB 4150:PRINT :PRINT "{white}Sell what?":GOSUB 3010:VJ=VAL(B$)-1 
5633 IF VJ<0 OR VJ>NG-1 THEN GOTO 5649 
5634 IF GC(VJ) = 0 THEN GOTO 5648 
5635 PRINT :PRINT "{white}How many?{yellow} ";:GOSUB 3010:VI=VAL(B$):IF VI=0 THEN GOTO 5647 
5636 IF (GC(VJ))<VI THEN GOTO 5646 
5637 PRINT "{orange}Sell ";VI;" ";GN$(VJ)"s for {green}$";INT(GP(VI)*.60);"{orange}?(Y/N)"; 
5638 GOSUB 5200:IF YN=0 THEN GOTO 5602 
5644 C=C+(VI*INT(GP(VJ)*.6)):GC(VJ)=GC(VJ)-VI 
5645 PRINT :PRINT "{cyan}Sold "+STR$(VI)+" ";GN$(VJ)+"s":GOSUB 5000:GOSUB 5180:GOTO 5602 
5646 GOSUB 58003: GOTO 5602 
5647 GOSUB 58001: GOTO 5602 
5648 PRINT :PRINT "{red}You don't have shit to sell!":GOSUB 5000: GOTO 5602 
5649 GOSUB 58000: GOTO 5602 
5650 IF A$<>"H" AND A$<>"h" THEN GOTO 5698 
5660 PRINT "Shoplift{$0d}" 
5661 IF INT(RND(1)*100)>30 THEN GOTO 5665 
5662 PRINT :PRINT "{red}You get caught{$0d}The clerk shoots you in the ass." 
5663 GOSUB 5000:H=H-20:IF H<=0 THEN GOTO 59000 
5664 GOSUB 5100:RETURN 
5665 IF INT(RND(1)*100)>70 THEN GOTO 5670 
5666 PRINT :PRINT "{red}You get caught{$0d}You manage to get away.":GOSUB 5000:RETURN 
5670 IF INT(RND(1)*100)>15 THEN GOTO 5696 
5672 J=-1:FOR I = 0 TO NG-1:IF J=-1 AND INT(RND(1)*100)<=70 THEN J=I 
5674 NEXT  
5675 IF J=-1 THEN J=0 
5676 PRINT :PRINT "{orange}You manage to steal a ";GN$(J):GOSUB 5000:GOSUB 5180:GC(J)=GC(J)+1 
5677 GOTO 5602 
5696 PRINT :PRINT "{red}You consider shoplifting{$0d}but don't risk it.":GOSUB 5000: GOTO 5602 
5698 IF A$<>"l" AND A$<>"L" THEN GOTO 5602 
5699 PRINT "Leave":GOSUB 5180:GOSUB 5000:RETURN 
'------------------------------------------------
'----Hospital
'------------------------------------------------
5700 PRINT :PRINT "{white}You enter the hospital.{$0d}It smells like chemicals." 
5701 VI = (100-H)*(INT(RND(1)*1000))+100 
5702 PRINT "You have {green}$";C;"{white}." 
5706 PRINT "{reverse on}D{reverse off}octor, {reverse on}S{reverse off}teal drugs, {reverse on}L{reverse off}eave: {yellow}"; 
5710 GOSUB 3030:GOSUB 3000 
5712 IF A$<>"d" AND A$<>"D" THEN GOTO 5730 
5714 PRINT "See Doctor{$0d}":IF H=100 THEN GOTO 5796 
5716 PRINT "{white}The doctor will fix you up" 
5717 PRINT "for {green}$";VI;"{white}.  Pay(Y/N)? {yellow}";:GOSUB 5200 
5718 IF YN=1 THEN GOTO 5724 
5721 GOSUB 5000: GOTO 5702 
5724 IF C<VI THEN GOTO 5797 
5726 C=C-VI:H=100:PRINT :PRINT "{orange}The doctor stitches you up.":GOSUB 5000: GOTO 5702 
5730 IF A$<>"s" AND A$<>"S" THEN GOTO 5798 
5734 PRINT "Steal drugs{$0d}" 
5761 IF INT(RND(1)*100)>30 THEN GOTO 5765 
5762 PRINT :PRINT "{red}You get caught{$0d}The guard shoots you in the ass." 
5763 GOSUB 5000:H=H-20:GOSUB 5100:IF H<=0 THEN GOTO 59000 
5764 RETURN 
5765 IF INT(RND(1)*100)>70 THEN GOTO 5770 
5766 PRINT :PRINT "{red}You get caught{$0d}You manage to get away.":GOSUB 5000:RETURN 
5770 IF INT(RND(1)*100)>15 THEN GOTO 5795 
5772 IF CL=0 THEN GOTO 5794 
5775 DC(4)=DC(4)+CL 
5776 PRINT :PRINT "{orange}You manage to steal ";CL;" ludes":GOSUB 5000:GOSUB 5100 
5777 GOTO 5702 
5794 PRINT :PRINT "{red}You find ludes,{$0d}but can't carry any more.":GOSUB 5000: GOTO 5702 
5795 PRINT :PRINT "{red}You consider stealing{$0d}but don't risk it.":GOSUB 5000: GOTO 5702 
5796 PRINT :PRINT "{red}You don't need a doctor, dumbass.":GOSUB 5000: GOTO 5702 
5797 GOSUB 58004: GOTO 5702 
5798 IF A$<>"l" AND A$<>"L" THEN GOTO 5702 
5799 PRINT "Leave":GOSUB 5000:RETURN 
'------------------------------------------------
'----Bank
'------------------------------------------------
5800 PRINT  
5801 PRINT "{white}You enter the bank.{$0d}The security guard eyes you." 
5802 PRINT :PRINT "{white}You have {green}$";B;"{white} in the bank." 
5803 PRINT "You're carrying {green}$";C;"{white}." 
5806 PRINT "{reverse on}W{reverse off}ithdraw, {reverse on}D{reverse off}eposit, {reverse on}L{reverse off}eave: {yellow}"; 
5810 GOSUB 3030:GOSUB 3000 
5812 IF A$<>"w" AND A$<>"W" THEN GOTO 5830 
5814 PRINT "Withdraw.{$0d}How much?";:GOSUB 3010:PS=VAL(B$) 
5816 IF PS=0 THEN GOTO 5802 
5818 IF PS>B THEN GOTO 5844 
5822 C=C+PS:B=B-PS: GOTO 5802 
5830 IF A$<>"D" AND A$<>"d" THEN GOTO 5850 
5831 PRINT "Deposit.{$0d}How much?";:GOSUB 3010:PS=VAL(B$) 
5832 IF PS=0 THEN GOTO 5802 
5833 IF PS>C THEN GOTO 5844 
5835 C=C-PS: B=B+PS 
5836 GOTO 5802 
5844 GOSUB 58004: GOTO 5802 
5848 RETURN 
5850 IF A$<>"l" AND A$<>"L" THEN GOTO 5802 
5852 PRINT "Leave":RETURN 
'------------------------------------------------
'---Combat with cops
'------------------------------------------------
6000 QI = INT(RND(1)*(NC-1)):QJ=INT(RND(1)*(DE(QI)-1)):QH = 100:QW=100 
6002 PRINT :PRINT "{red}";CN$(QI);" and {$0d}";QJ;" deputies are chasing you" 
6010 PRINT :PRINT "{white}{reverse on}R{reverse off}un, {reverse on}F{reverse off}ight, {reverse on}S{reverse off}urrender:{yellow}" 
6020 GOSUB 3030:GOSUB 3000 
6022 IF A$<>"R" AND A$<>"r" THEN GOTO 6100 
6032 PRINT "Run{$0d}" 
6035 IF INT(RND(1)*100) < 50 THEN GOTO 6038 
6037 PRINT "{red}You couldn't get away!": GOTO 6300 
6038 PRINT "{orange}You got away!":RETURN 
6099 GOTO 6010 
6100 IF A$<>"F" AND A$<>"f" THEN GOTO 6200 
6110 PRINT "Fight{$0d}": GOTO 6300 
6199 GOTO 6010 
6200 IF A$<>"S" AND A$<>"s" THEN GOTO 6010 
6210 PRINT "Surrender{$0d}" 
6212 IF INT(RND(1)*100)>10 THEN GOTO 6214 
6213 PRINT "{orange}You slip away while they're running{$0d} your fake ID!": GOTO 6038 
6214 IF INT(RND(1)*100)>10 THEN GOTO 6216 
6215 PRINT "{orange}You shit yourself and they don't want{$0d} you in their car!": GOTO 6038 
6216 IF INT(RND(1)*100)>10 THEN GOTO 6220 
6217 PRINT "{red}The cops kick your ass and leave you in a ditch." 
6218 H=H-30:IF H<=0 THEN GOTO 59000 
6219 RETURN 
6220 IF INT(RND(1)*100)>30 THEN GOTO 6235 
6230 PRINT "{red}You manage to ditch all your drugs{$0d}before they search you." 
6232 FOR I = 0 TO ND-1:DC(I)=0:NEXT I:RETURN 
6235 IF INT(RND(1)*100)>40 THEN GOTO 6240 
6237 PRINT "{red}The cops take all of your money as{$0d}a bribe, but let you go." 
6238 C = 0: RETURN 
6240 PRINT "{red}The cops take all of your drugs and{$0d}money, but let you go." 
6242 C = 0:FOR I = 0 TO ND-1:DC(I)=0:NEXT I:RETURN 
6299 GOTO 6010 
'------------------------------------------------
'----Combat
'---QT (Their Health)= 100 + 100* Deputies
'--QH (Damage Ticker)= 100
'------------------------------------------------
'-- Foreach deputy + 1 , roll CH(QI), if hit, damage is max GD(3)
'-- Sub from QH, if < 0 THen 1 bitch died, if no more bitches player
'-- receives damage
'-- For attacking, best weapons are allocated first
6300 REM  
'---Cop Attack Round
6304 PRINT :PRINT "{red}They shoot at you..."; 
6305 IF INT(RND(1)*100)>CH(QI) THEN GOTO 6360 
'---How many hits?
6306 QY = INT(RND(1)*QJ+1) 
6310 QD = INT(RND(1)*GD(3))*QY 
6355 IF NB > 0 THEN GOTO 6365 
6358 PRINT "and hit you!":H=H-QD:GOSUB 5100:IF H<=0 THEN GOTO 59000 
6359 GOTO 6400 
6360 PRINT "{orange}and miss!": GOTO 6400 
6365 QH=QH-QD: IF QH<=0 THEN GOTO 6375 
6370 PRINT "{$0d}and hit one of your bitches!": GOTO 6400 
6375 PRINT "{$0d}and killed one of your bitches!":NB=NB-1:QH=100 
6376 GOSUB 5100 
6377 GOTO 6400 
'---Player attack Round, update Firepower (QP)
6400 GOSUB 7050 : IF QP = 0 THEN GOTO 6500 
6401 PRINT :PRINT "{cyan}You fire..."; 
6402 IF INT(RND(1)*100) > TH(QI) THEN GOTO 6460 
6406 QY = INT(RND(1)*NB+1) 
6410 QD = INT((QP/(NB+1))*QY) 
6455 IF QJ> 0 THEN GOTO 6465 
6458 PRINT "{$0d}{orange}and hit ";CN$(QI);"!":QW=QW-QD:IF QW<=0 THEN GOTO 6550 
6459 GOTO 6010 
6460 PRINT "{red}and miss!": GOTO 6010 
6465 QW=QW=QD: IF QW<=0 THEN GOTO 6475 
6470 PRINT "{$0d}{orange}and hit one of the deputies!": GOTO 6010 
6475 PRINT "{$0d}{orange}and kill one of the deputies!":QJ=QJ-1:QW=100: GOTO 6010 
6500 PRINT "{red}{$0d}You throw some rocks and miss.": GOTO 6010 
6550 PRINT "{white}You killed ";CN$(QI);"!":QC=INT(RND(1)*(CD(QI)*10000))+1000 
6555 PRINT "You found {green}$";QC;"{white} on the corpses.":GOSUB 5000:RETURN 
6999 RETURN 
'------------------------------------------------
'---Remove worst gun from player inventory
'------------------------------------------------
7000 IF G < (NB+1) THEN RETURN 
7001 FOR I = 0 TO NG-1 
7002 IF GC(I)>0 THEN GOTO 7005 
7003 NEXT  
7004 RETURN 
7005 GC(I)=GC(I)-1 
7006 RETURN 
'------------------------------------------------
'---UPdate QP (Player Firepower in combat)
'---Best guns are allocated first.
'------------------------------------------------
7050 QZ = NB+1:FOR I = NG-1 TO 0 STEP -1 
7055 IF GC(I)= 0 THEN GOTO 7065 
7057 FOR J = 1 TO GC(I):QP=QP+GD(I):QZ=QZ-1 
7059 IF QZ= 0 THEN RETURN 
7060 NEXT J 
7065 NEXT I 
7067 RETURN 
7100 J=0:FOR I = 0 TO NG-1:J=J+GC(I):NEXT :RETURN 
'------------------------------------------------
'---Jail
'------------------------------------------------
8000 REM JAIL 
'------------------------------------------------
'---Ending, ran out of time.
'------------------------------------------------
57000 GOSUB 59100 
57010 PRINT "{clear}Time's up!" 
57020 GOTO 59010 
'------------------------------------------------
'----Common Messages
'------------------------------------------------
58000 PRINT :PRINT "{red}You can't afford that many.":GOSUB 5000:RETURN 
58001 PRINT :PRINT "{red}Invalid Selection":GOSUB 5000:RETURN 
58002 PRINT :PRINT "{red}You can't carry that much!":GOSUB 5000:RETURN 
58003 PRINT :PRINT "{red}You don't have that many!":GOSUB 5000:RETURN 
58004 PRINT :PRINT "{red}You don't have that much!":GOSUB 5000:RETURN 
'------------------------------------------------
'---Ending, Died
'------------------------------------------------
59000 PRINT :PRINT"{red}You died."
'Really fuck their score for dying.  
59001 GOSUB 59100:SC=SC/2:SC=SC-10000:IF SC<0 THEN SC = 0 
59010 PRINT "Final Score: ";SC 
59020 GOTO 59020 
'------------------------------------------------
'---Figure Score
'------------------------------------------------
59100 SC = C + B 
59106 FOR I = 0 TO ND-1:SC = SC + (LP(I) * DC(I)):NEXT  
59107 SC = SC + (NB * 10000) 
59108 FOR I = 0 TO NG-1:SC = SC + (GC(I)*GP(I)):NEXT  
59110 PRINT "Ending Score: "+SC 
59120 RETURN 
'------------------------------------------------
'------End of Main Code
'--------------------------------------------------
59999 RETURN 
'------------------------------------------------
'-----Initialize Game Variables
'------------------------------------------------
'-----Number of Locations, Number of Drugs, Number of cops, Number of guns
60000 NN=8:ND=12:NC=3:NG=6 
'-----Drugname,locname,selectionnum for locdrug,cancheap,canexpensive,selectionnum for inventory drug
60010 DIM DN$(ND):DIM LO$(NN):DIM PN(ND):DIM CC(ND):DIM CE(ND):DIM PI(ND) 
'-----AveragePrice,Rarity,Gun Names,Gun Damages, Gun Prices, Guns Carried
60015 DIM AP(ND):DIM DR(ND):DIM GN$(NG):DIM GD(NG):DIM GP(NG):DIM GC(NG) 
'------HASDRUG,LOPRICE,DCOST,HIPRICE,cheapstring
60024 DIM HD(ND):DIM LP(ND):DIM DC(ND):DIM HP(ND):DIM CD$(ND):DIM DP(ND)
'------CurrentLocation,Cash,Debt,Day,Bank
60026 CN = 0:C=2000:D=2000:DA=1:BK=0
'------MinDrugs,MaxDrugs,PoliceEncounter, BItch HIre offer
60031 DIM MD(NN):DIM XD(NN):DIM PP(NN):DIM BO(NN)
'------Set Capacity,Health,Bitches,Total Carried, Carry Left,Guns
60040 CP = 100:H = 100:NB=0:TD=0:CL=100:G=0
'------Cop Names, Cop ToHit, CopDeputies, Player ToHit Cop
60044 DIM CN$(NC):DIM CH(NC):DIM DE(NC):DIM TH(NC)
'------------------------------------------------
'-----Load Drug Data Arrays
'------------------------------------------------
60070 FOR I = 0 TO ND-1:READ DN$(I):NEXT
60071 FOR I = 0 TO ND-1:READ LP(I):NEXT
60072 FOR I = 0 TO ND-1:READ HP(I):NEXT
60073 FOR I = 0 TO ND-1:READ CC(I):NEXT
60074 FOR I = 0 TO ND-1:READ CE(I):NEXT
60075 FOR I = 0 TO ND-1:READ DR(I):NEXT
'------------------------------------------------
'-----Load Location Data Arrays
'------------------------------------------------
60081 FOR I = 0 TO NN-1:READ LO$(I):NEXT
60082 FOR I = 0 TO NN-1:READ MD(I):NEXT
60083 FOR I = 0 TO NN-1:READ PP(I):NEXT
60084 FOR I = 0 TO NN-1:READ XD(I):NEXT
60085 FOR I = 0 TO NN-1:READ BO(I):NEXT
'------------------------------------------------
'----Load Cop Data Arrays
'------------------------------------------------
60090 FOR I = 0 TO NC-1:READ CN$(I):NEXT 
60091 FOR I = 0 TO NC-1:READ CH(I):NEXT
60092 FOR I = 0 TO NC-1:READ DE(I):NEXT
60093 FOR I = 0 TO NC-1:READ TH(I):NEXT
'------------------------------------------------
'----Load Gun Data Arrays
'------------------------------------------------
60100 FOR I = 0 TO NG-1:READ GN$(I):NEXT
60101 FOR I = 0 TO NG-1:READ GP(I):NEXT
60102 FOR I = 0 TO NG-1:READ GD(I):NEXT
60103 FOR I = 0 TO NG-1:GC(I)=0:NEXT
'------------------------------------------------
'-----Drug Data, Names
'------------------------------------------------
61001 DATA "acid","cocaine","hash","heroin","ludes","molly","opium","pcp"
61002 DATA "crack","shrooms","meth","weed"
'------------------------------------------------
'------Drug Data, Minimum Normal Prices
'------------------------------------------------
61004 DATA 1000,15000,480,5500,11,1500,540,1000,220,630,90,315
'------------------------------------------------
'------Drug Data, Maximum Normal Prices
'------------------------------------------------
61006 DATA 4400,29000,1280,13000,60,4400,1250,2500,700,1300,250,890
'------------------------------------------------
'-----Drug Data, Can be Cheap
'------------------------------------------------
61008 DATA 1,0,1,0,1,0,0,0,0,0,0,1
'------------------------------------------------
'-----Drug Data, Can be Expensive
'------------------------------------------------
61009 DATA 0,1,0,1,0,0,1,0,0,0,1,0
'------------------------------------------------
'-----Drug Data, Rarity (Chance of presence)
'------------------------------------------------
61010 DATA 40,60,40,50,50,50,50,50,70,30,80,100
'------------------------------------------------
'-----Location Data, Location Names (LO$[])
'------------------------------------------------
61021 DATA "The Bronx","The Ghetto","Central Park","Manhattan","Coney Island" 
61022 DATA "Brooklyn","Queens","Staten Island" 
'------------------------------------------------
'-----Location Data, Minimum # of drugs (MD[])
'------------------------------------------------
61024 DATA 7,8,6,4,6,4,6,6 
'------------------------------------------------
'-----Location Data, Police Encounter Chance (PP[])
'------------------------------------------------
61026 DATA 10,5,15,90,20,70,50,20 
'------------------------------------------------
'-----Location Data, Maximum # of drugs (XD[])
'------------------------------------------------
61028 DATA 12,12,12,10,12,11,12,12 
'------------------------------------------------
'----Location Data, Bitch Offer Chance
'------------------------------------------------
61029 DATA 20,25,5,6,7,15,5,5 
'------------------------------------------------
'-----Misc strings
'------------------------------------------------
61041 CD$(0)="{light blue}The market is flooded with cheap, {$0d}home-made acid!" 
61042 CD$(2)="{light green}The Marakesh Express has arrived!" 
61043 CD$(4)="{gray 3}Some guys raided a pharmacy,{$0d}and are selling cheap ludes!" 
61044 CD$(11)="{pink}Columbian freighters dusted the Coast{$0d}Guard! Weed prices have" 
61045 CD$(11)=CD$(11)+" bottomed out!" 
61050 B1$ = "bitch": B2$="bitches" 
61057 E1$="{yellow}Cops made a big ":E2$=" bust!"+CHR$(13)+"Prices are outrageous!" 
61058 E3$="{purple}Addicts are buying ":E4$="{$0d}at ridiculous prices!" 
'------------------------------------------------
'------Lady on the Subway
'------------------------------------------------
61060 NL=29:DIM LS$(NL) 
61061 LS$(0)="Wouldn't it be funny if everyone{$0d}suddenly quacked at once?{$00}
61062 LS$(1)="The Pope was once Jewish, you know." 
61063 LS$(2)="I'll bet you have some{$0d}really interesting dreams." 
61064 LS$(3)="So I think...{$0d}I'm going to Amsterdam this year." 
61065 LS$(4)="Son, you need a yellow haircut." 
61066 LS$(5)="It's wonderful what they're{$0d}doing with incense these days." 
61067 LS$(6)="I wasn't always a woman, you know." 
61068 LS$(7)="Does your mother know{$0d}you're a dope dealer?" 
61069 LS$(8)="Are you high on something?" 
61070 LS$(9)="Oh, you must be from California." 
61071 LS$(10)="I used to be a hippie, myself." 
61072 LS$(11)="There's nothing like{$0d}having lots of money." 
61073 LS$(12)="You look like an aardvark!" 
61074 LS$(13)="I don't believe in Ronald Reagan." 
61075 LS$(14)="Courage!  Bush is a noodle!" 
61076 LS$(15)="Haven't I seen you on TV?" 
61077 LS$(16)="I think hemorrhoid commercials{$0d}are really neat!" 
61078 LS$(17)="We're winning the war for drugs!" 
61079 LS$(18)="A day without dope is like night." 
61080 LS$(19)="We only use 20% of our brains,{$0d}why not burn out the other 80%?" 
61081 LS$(20)="I'm soliciting contributions{$0d}for Zombies for Christ." 
61082 LS$(21)="I'd like to sell you an edible poodle." 
61083 LS$(22)="Winners don't do drugs...{$0d}unless they do." 
61084 LS$(23)="I am the walrus!" 
61085 LS$(24)="I feel an unaccountable urge{$0d}to dye my hair blue." 
61086 LS$(25)="Wasn't Jane Fonda wonderful{$0d}in Barbarella?" 
61087 LS$(26)="Just say No... well, maybe...{$0d}ok, what the hell!" 
61088 LS$(27)="Would you like a jelly baby?" 
61089 LS$(28)="Drugs can be your friend!" 
'------------------------------------------------
'-----Stopped TOs
'------------------------------------------------
61100 NS=5:DIM ST$(NS):ST$(0)="have a beer." 
61101 ST$(1)="smoke a joint." 
61102 ST$(2)="smoke a cigar." 
61103 ST$(3)="smoke a Djarum." 
61104 ST$(4)="smoke a cigarette." 
61105 ST$(5)="wank." 
'------------------------------------------------
'-----Music Playing
'------------------------------------------------
61200 NM=17:DIM MP$(NM) 
61201 MP$(0)= "'Are you Experienced'{$0d}by Jimi Hendrix." 
61202 MP$(1)= "'Cheeba Cheeba'{$0d}by Tone Loc." 
61203 MP$(2)= "'Comin' in to Los Angeles'{$0d}by Arlo Guthrie." 
61204 MP$(3)= "'Commercial'{$0d}by Spanky and Our Gang." 
61205 MP$(4)= "'Late in the Evening'{$0d}by Paul Simon." 
61206 MP$(5)= "'Light Up'{$0d}by Styx." 
61207 MP$(6)= "'Mexico'{$0d}by Jefferson Airplane." 
61208 MP$(7)= "'One toke over the line'{$0d}by Brewer & Shipley." 
61209 MP$(8)= "'The Smokeout'{$0d}by Shel Silverstein." 
61210 MP$(9)= "'White Rabbit'{$0d}by Jefferson Airplane." 
61211 MP$(10)="'Itchycoo Park'{$0d}by Small Faces." 
61212 MP$(11)="'White Punks on Dope'{$0d}by the Tubes." 
61213 MP$(12)="'Legend of a Mind'{$0d}by the Moody Blues." 
61214 MP$(13)="'Eight Miles High'{$0d}by the Byrds." 
61215 MP$(14)="'Acapulco Gold'{$0d}by Riders of the Purple Sage." 
61216 MP$(15)="'Kicks'{$0d}by Paul Revere & the Raiders." 
61217 MP$(16)="'Legalize It'{$0d}by Mojo Nixon & Skid Roper." 
61218 MP$(17)="'Cheeba Cheeba'{$0d}by Tone Loc." 
'------------------------------------------------
'---Cops - Names, Toughness, Max Deputies, ToHit, 
'------------------------------------------------
61300 DATA "Officer Hardass","Officer Lardass", "Sergent Stedanko" 
61301 DATA 50,30,60 
61302 DATA 5,3,8 
61304 DATA 50,60,50 
'------------------------------------------------
'-------Cursing in the pub
'------------------------------------------------
61400 NW = 14:DIM C0$(NW):DIM C1$(NW) 
61401 C0$(0)="shit":C0$(1)="fuck":C0$(2)="cock":C0$(3)="cunt":C0$(4)="piss" 
61402 C0$(5)="hell":C0$(6)="damn":C0$(7)="ass":C0$(8)="bastard":C0$(9)="felch" 
61403 C1$(0)="cocks":C1$(1)="holes":C1$(2)="hamsters":C1$(3)="knuckles" 
61404 C1$(4)="in the ass":C1$(5) ="your mom":C1$(6)="in hell":C1$(7)="dot com" 
61405 C1$(8)="upside your head":C1$(9)="motherfucker":C1$(10)="squirrels" 
61406 C0$(10)="goddamn":C0$(11)="assfucking":C0$(12)="motherfucking" 
61407 C0$(13)="fucked up":C1$(11)="donkey balls":C1$(12)="horsefucker" 
61408 C1$(13)="cocksucker" 
'----------------------------------------------
'-----Gun Data
'----------------------------------------------
61500 DATA ".22 revolver",".38 special", "9mm pistol","12ga shotgun" 
61501 DATA "AK47","rocket launcher" 
61502 DATA 1000,2000,3000,4000,10000,20000 
61503 DATA 10,20,30,40,60,80 
'----------------------------------------------
62000 RETURN 
'----------------------------------------------
