#include <vector>
#include <iostream>
#include <iomanip>
#include <algorithm>

using namespace std;

template <typename T>

void Sequence(T* A, int n)
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
	int M;
	for (int i = 0; i < n; i++)
	{ 
		M = i;
		for (int j = i + 1; j < n; j++)
		{
			if (A[j] < A[M])
				M = j;
		}
		swap(A[i], A[M]);
	}

	for (int i = 0; i < n; i++)
	{
		cout << A[i] << " ";
	}
}

void ShellSort(double* A, int n)
{
	double temp;
	int step = n / 2;
	while(step)
	{
		for (int i = step; i < n; i++)
		{
			for (int j = i - step; j >= 0; j -= step)
			{
				if (A[j] > A[j + step])
				{
					temp = A[j];
					A[j] = A[j + step];
					A[j + step] = temp;
				}
			}
		}
		step /= 2;
	}
	for (int i = 0; i < n; i++)
	{
		cout << A[i] << " ";
	}
}

int GetDig(int num, int dig)
{
	return (num / (int)(pow(10, dig))) % 10;
}

void Counting(int* B, int n, int dig)
{
	const int base = 10;
	int* mas = new int[base];
	int* out = new int[n];

	for (int i = 0; i < base; i++)
		mas[i] = 0;

	for (int i = 0; i < n; i++) {
		int d = GetDig(B[i], dig);
		mas[d]++;
	}

	for (int i = 1; i < base; i++) {
		mas[i] += mas[i - 1];
	}

	for (int i = n - 1; i >= 0; --i) {
		int d = GetDig(B[i], dig);
		out[mas[d] - 1] = B[i];
		mas[d]--;
	}

	for (int i = 0; i < n; i++) {
		B[i] = out[i];
	}

	delete[] mas;
	delete[] out;
}

void RadixSort(int* B, int n)
{
	int mx = B[0];
	int digs = 0;
	for (int i = 0; i < n; i++)
	{
		if (B[i] > mx)
			mx = B[i];
	}

	while (mx != 0) {
		digs++;
		mx /= 10;
	}

	for (int d = 0; d < digs; d++) {
		Counting(B, n, d);
	}

	for (int i = 0; i < n; i++)
	{
		cout << B[i] << " ";
	}
}

void HeapSort(vector <double> A)
{
	make_heap(A.begin(), A.end());
	for (auto i = A.end(); i != A.begin(); --i)
	{
		pop_heap(A.begin(), i);
	}

	for (double x : A)
		cout << x << " ";
}

void Merging(double* A, double* M, int begin, int end)
{
	if (begin < end)
	{
		int mid = (begin + end) / 2;
		Merging(A, M, begin, mid);
		Merging(A, M, mid + 1, end);

		int k = begin;
		for (int i = begin, j = mid + 1; i <= mid || j <= end; )
		{
			if (j > end || (i <= mid && A[i] < A[j]))
			{
				M[k] = A[i];
				++i;
			}
			else {
				M[k] = A[j];
				++j;
			}
			++k;
		}
		for (int i = begin; i <= end; ++i)
		{
			A[i] = M[i];
		}
	}
}

void MergeSort(double* A, int n)
{
	if (n != 0)
	{
		double* M = new double[n];
		Merging(A, M, 0, n-1);
	}

	for (int i = 0; i < n; i++)
	{
		cout << A[i] << " ";
	}

}

int main()
{
	setlocale(LC_ALL, "RU");

	int choose;

	cout << "Выберите вид сортировки (число от 1 до 9): ";
	cin >> choose;


	switch (choose)
	{
	case 1: //Расчесочка
	{
		int n;
		cout << "Введите кол-во чисел последовательности: ";
		cin >> n;
		double* A = new double[n];
		Sequence(A, n);
		cout << "Сортировка расческой: ";
		CombSort(A, n);
		cout << endl;
		delete[] A;
		break;
	}
	case 2: //Вставками
	{
		int n;
		cout << "Введите кол-во чисел последовательности: ";
		cin >> n;
		double* A = new double[n];
		Sequence(A, n);
		cout << "Сортировка вставками: ";
		InsertSort(A, n);
		cout << endl;
		delete[] A;
		break;
	}
	case 3: //Выбором
	{
		int n;
		cout << "Введите кол-во чисел последовательности: ";
		cin >> n;
		double* A = new double[n];
		Sequence(A, n);
		cout << "Сортировка выборкой: ";
		SelSort(A, n);
		cout << endl;
		delete[] A;
		break;
	}
	case 4: //Шелла
	{
		int n;
		cout << "Введите кол-во чисел последовательности: ";
		cin >> n;
		double* A = new double[n];
		Sequence(A, n);
		cout << "Сортировка Шелла: ";
		ShellSort(A, n);
		cout << endl;
		delete[] A;
		break;
	}
	case 5: //Поразрядная
	{
		int n;
		cout << "Примечание: данная сортировка работает с целыми числами." << endl;
		cout << endl;
		cout << "Введите кол-во чисел последовательности: ";
		cin >> n;
		int* B = new int[n];

		Sequence(B, n);

		cout << "Поразрядная сортировка: ";
		RadixSort(B, n);
		cout << endl;

		delete[] B;
		break;
	}
	case 6:
	{
		cout << "Введите кол-во чисел последовательности: ";
		int n;
		double x;
		cin >> n;
		cout << "Введите последовательность чисел: ";
		vector<double> A;
		for (int i = 0; i < n; i++)
		{
			cin >> x;
			A.push_back(x);
		}
		cout << "Пирамидальная сортировка: ";
		HeapSort(A);
		break;
	}
	case 7:
	{
		cout << "Введите кол-во чисел последовательности: ";
		int n;
		cin >> n;
		double* A = new double[n];
		Sequence(A, n);

		cout << "Сортировка слиянием: ";
		MergeSort(A, n);
		delete[] A;
		break;
	}
	case 8:
	{
		cout << "Лаба в разработке :)" << endl;
		break;
	}
	case 9:
	{
		cout << "Лаба в разработке :)" << endl;
		break;
	}
	default:
	{
		cout << "Вы ввели некорректный номер (введите от 1 до 9)" << endl;
		break;
	}
	}


	return 0;
}