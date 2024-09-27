
#include <iostream>
#include <cmath>

using namespace std;

double pi = acos(-1);



struct Answer {
	double g_M;
	double g_FM;
	double g_i;

	Answer() = default;

	Answer(double M, double FM, double i)
	{
		g_M = M;
		g_FM = FM;
		g_i = i;
	}

	/*void print()
	{
		cout << "Корень функции = " << g_M << endl;
		cout << "Количество шагов: " << g_i << endl;
		cout << "Вычисленная погрешность = " << g_FM << endl;
	}*/
	double getM()
	{
		return g_M;
	}
	double getFM()
	{
		return g_FM;
	}
	double getI()
	{
		return g_i;
	}


	void print()
	{
		cout << g_M << ", " << g_i << ", " << g_FM << '\t';
	}
};

double F(double x)
{

	return cos(2 * x + pi / 2) + pow(x,3) + 8;
}

double Line(double (*f)(double), double L, double R)
{
	return (L * f(R) - f(L) * R) / (f(R) - f(L));
}



Answer FindingHALF(double (*f)(double), double e)
{
	double L = -10;
	double R = -10;

	int i = 0;
	double M = (L + R) / 2;
	if (F(L) < 0)
	{
		while (F(R) < 0)
			R += 0.5;
		M = (L + R) / 2;
		while (abs(F(R) - F(L)) > e) {
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
		while (abs(F(R) - F(L)) > e) {
			if (F(L) * F(M) > 0) L = M;
			else R = M;
			M = (L + R) / 2;
			i++;
		}
	}
	return Answer(M, abs(F(M)), i);
}

double Different(double (*f)(double), double L, double R)
{
	if (f(L) * f(R) < 0) return true;
	else return false;
}

double FindPoints()
{
	double L = 0;
	double R = 0;

	while (!Different(F, L, R))
	{
		L -= 0.5;
		R += 0.5;
	}
	return L;
}

Answer FindingSEC(double (*f)(double), double e)
{
	int i = 1;
	double L = FindPoints();
	double R = -L;
	double M = Line(F, L, R);
	while (abs(F(M)) > e)
	{
		if (F(M) * F(L) > 0) L = M;
		else R = M;
		M = Line(F, L, R);
		i++;
	}
	return Answer(M, abs(F(M)), i);
}

double RatioK(double (*f)(double), double e)
{
	return FindingHALF(f, e).getI() / FindingSEC(f, e).getI();
}

int main()
{
	setlocale(LC_ALL, "RU");
	
	//Answer x;
	cout << "\t\t  E = 0.1  \t\t\tE = 0.01  \t\t\tE = 0.0001" << endl;
	cout << "М. Пол. Деления:  ";
	FindingHALF(F, 0.1).print();
	cout << '\t';
	FindingHALF(F, 0.01).print();
	cout << '\t';
	FindingHALF(F, 0.0001).print();
	cout << endl;
	cout << "Метод секущей:    ";
	FindingSEC(F, 0.1).print();
	cout << '\t';
	FindingSEC(F, 0.01).print();
	cout << '\t';
	FindingSEC(F, 0.0001).print();
	cout << endl;
	cout << "\t\t" << RatioK(F, 0.1);
	cout << "\t\t\t\t" << RatioK(F, 0.01);
	cout << "\t\t\t\t" << RatioK(F, 0.0001);



	return 0;
}
