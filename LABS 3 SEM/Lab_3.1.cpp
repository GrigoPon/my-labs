#include <iostream>
#include <cmath>

using namespace std;

double pi = acos(-1);



struct Answer {
	double g_M;
	double g_FM;
	int g_i;

	Answer() = default;

	Answer(double M, double FM, int i)
	{
		g_M = M;
		g_FM = FM;
		g_i = i;
	}

	void print()
	{
		cout << "Корень функции = " << g_M << endl;
		cout << "Количество шагов: " << g_i  << endl;
		cout << "Вычисленная погрешность = " << g_FM << endl;
	}
};

double F(double x)
{
	
	return cos(2 * x + pi / 2) - x + 8;
}

Answer Finding ()
{
	double L = -10;
	double e1 = 0.01;
	double e2 = 0.001;
	double R = -10;

	int i = 0;
	double M = (L+R)/2;
	if (F(L) < 0)
	{
		while (F(R) < 0)
			R += 0.5;
		M = (L + R) / 2;
		while (abs(F(R) - F(L)) > e1) {
			if (F(L) * F(M) > 0) L = M;
			else R = M;
			M = (L + R) / 2;
			i++;
		}

	}
	else if (F(L) > 0)
	{
		while (F(R) > 0)
			R += 0.5;
		
		M = (L + R) / 2;
		while (abs(F(R) - F(L)) > e1) {
			if (F(L) * F(M) > 0) L = M;
			else R = M;
			M = (L + R) / 2;
			i++;
		}
	}
	return Answer(M,abs(F(M)),i);
}

int main()
{
	setlocale(LC_ALL, "RU");
	double L = -10;
	double e1 = 0.01;
	double e2 = 0.001;
	double R = -10;
	Answer x;

	x = Finding();
	x.print();
	
	return 0;
}