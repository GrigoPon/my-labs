#include <vector>
#include <iostream>
#include <iomanip>

using namespace std;

void Sequence(double* A, int n)
{	
	cout << endl;
	cout << "Введите последовательность: ";
	for (int i = 0; i < n; i++)
		cin >> A[i];
}

void CombSort(double* A, int n)
{
	const double factor = 1.247;
	double step = n - 1;
	while (step >= 1)
	{
		for (int i = 0; i + step < n; ++i)
		{
			if (A[i] > A[(int)(i + step)])
			{
				swap(A[i], A[(int)(i + step)]);
			}
		}
		step /= factor;
	}

	for (int i = 0; i < n; i++)
	{
		cout << A[i] << " ";
	}
}

void InsertSort(double* A, int n)
{
	for (int i = 1; i < n; i++)
	{
		double x = A[i];
		int j = i;
		while (j > 0 && A[j - 1] > x)
		{
			A[j] = A[j - 1];
			j--;
		}
		A[j] = x;
	}
	for (int i = 0; i < n; i++)
	{
		cout << A[i] << " ";
	}
}

void SelSort(double* A, int n)
{
	double x;
	for (int i = 0; i < n; i++)
	{
		for (int j = 1; j < n; j++)
		{
			if (A[i] > A[j])
			{
				if (A[j] > A[j + 1])
				{
					x = A[j + 1];
				}
			}
			swap(A[i], x);
		}
	}
	for (int i = 0; i < n; i++)
	{
		cout << A[i] << " ";
	}
}

int main()
{
	setlocale(LC_ALL, "RU");
	int n;
	cout << "Введите кол-во чисел последовательности: ";
	cin >> n;
	double* A = new double[n];
	Sequence(A, n);

	cout << "Сортировка расческой: ";
	CombSort(A, n);
	cout << endl;
	cout << "Сортировка вставками: ";
	InsertSort(A, n);
	cout << endl;
	cout << "Сортировка выборкой: ";
	SelSort(A, n);
	

	return 0;
}