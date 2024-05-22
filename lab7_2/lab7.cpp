#include <iostream>

using namespace std;
template <typename T>

class Matrix
{
public:
	int n;
	int m;
	T** mat;


	Matrix() = default;
	~Matrix() = default;

	Matrix(int N, int M, T* MAT)
	{
		n = N;
		m = M;
		mat = new T * [n];

		for (int i = 0; i < n; i++)
		{
			mat[i] = new T[m];
			for (int j = 0; j < m; j++)
				mat[i][j] = MAT[i][j];
		}

	}

	Matrix(const Matrix& other)
	{
		n = other.n;
		m = other.m;
		mat = new T * [n];
		for (int i = 0; i < n; i++)
			mat[i] = new T[m];

		for (int i = 0; i < n; i++)
			for (int j = 0; j < m; j++)
				mat[i][j] = other.mat[i][j];
	}

	Matrix& operator=(const Matrix& other)
	{
		n = other.n;
		m = other.m;
		mat = new T * [n];
		for (int i = 0; i < n; i++)
			mat[i] = new T[m];

		for (int i = 0; i < n; i++)
			for (int j = 0; j < m; j++)
				mat[i][j] = other.mat[i][j];
		return *this;
	}

	friend istream& operator>>(istream& in, Matrix& MatCin)
	{
		int N;
		int M;
		in >> N >> M;
		for (int i = 0; i < N; i++)
			for (int j = 0; j < M; j++)
				in >> MatCin[i][j];

		return in;
	}

	Matrix operator+(const Matrix& other)
	{
		//T* matr = new T[n * m];
		Matrix result(n, m);
		if (n == other.n || m == other.m)
		{
			for (int i = 0; i < n; i++)
				for (int j = 0; j < m; j++)
					result.mas[i][j] = mas[i][j] + other.mas[i][j];
			return result;
		}
		else
			cout << "сложение выполнить невозможно, так как размеры не соответствуют (по правилу сложения матриц)"
			return 9999;
	}
	
	Matrix operator-(const Matrix& other)
	{
		Matrix result(n, m);
		if (n == other.n || m == other.m)
		{
			for (int i = 0; i < n; i++)
				for (int j = 0; j < m; j++)
					result.mas[i][j] = mas[i][j] - other.mas[i][j];
			return result;
		}
		else
			cout << "сложение выполнить невозможно, так как размеры не соответствуют (по правилу сложения матриц)"
			return 9999;
	}



};

int main()
{
   
}

