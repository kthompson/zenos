int func(int a)
{
  int z = 0x4000;
  int x = 0x4000;
  int y = 0x4000;
  int w = 0x4000;
  int v = 0x4000;
  int u = 0x4000;
  int t = 0x4000;
	return u;
}
/*

each int

stack space for n variables
S mod 8 == 0;
S(n) = (n*4)

int fun0()
{
  return 5;
}

int fun1()
{
  int x = 7;
  return x + 5;
}

int fun2()
{
  int x = 7;
  int y = x + 6;
  return y + 5;
}

int fun3()
{
  int x = 7;
  int y = x + 6;
  int z = y + 5;
  return z + 4;
}

int fun4()
{
	int w = 8;
	int x = w + 7;
	int y = x + 6;
	int z = y + 5;
	return z + 4;
}

int fun5()
{
	int v = 9;
	int w = v + 8;
	int x = w + 7;
	int y = x + 6;
	int z = y + 5;
	return z + 4;
}


int function1(int a, int b)
{
  return a*b;
}
*/
