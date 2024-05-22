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

	Matrix(int N, int M)
	{
		n = N;
		m = M;
		mat = new T * [n];

		for (int i = 0; i < n; i++)
			mat[i] = new T[m];

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
		for (int i = 0; i < n; i++)
			delete[] mat[i];
		delete[] mat;

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
		for (int i = 0; i < MatCin.n; i++)
			for (int j = 0; j < MatCin.m; j++)
				in >> MatCin.mat[i][j];

		return in;
	}

	friend ostream& operator<<(ostream& os, Matrix& MatCin)
	{
		for (int i = 0; i < MatCin.n; i++)
		{
			for (int j = 0; j < MatCin.m; j++)
			{
				os << MatCin.mat[i][j] << " ";
			}	
			os << endl;
		}
		return os;
	}

	Matrix operator+(const Matrix& other)
	{
		//T* matr = new T[n * m];
		Matrix result(n, m);
		if (n == other.n && m == other.m)
		{
			for (int i = 0; i < n; i++)
				for (int j = 0; j < m; j++)
					result.mat[i][j] = mat[i][j] + other.mat[i][j];
			return result;
		}
		else
		{
			cout << "сложение выполнить невозможно, так как размеры не соответствуют (по правилу сложения матриц)" << endl;
			Matrix Error(1, 1);
			T a = 9999;
			Error.mat[0][0] = a;
			return Error;
		}
	}

	Matrix operator-(const Matrix& other)
	{
		Matrix result(n, m);
		if (n == other.n && m == other.m)
		{
			for (int i = 0; i < n; i++)
				for (int j = 0; j < m; j++)
					result.mat[i][j] = mat[i][j] - other.mat[i][j];
			return result;
		}
		else
		{
			Matrix Error(1, 1);
			cout << "сложение выполнить невозможно, так как размеры не соответствуют (по правилу сложения матриц)" << endl;
			T a = 9999;
			Error.mat[0][0] = a;
			return Error;
		}
	}

	Matrix operator+=(const Matrix& other)
	{
		Matrix result = *this + other;
		*this = result;
		return *this;
	}

	Matrix operator-=(const Matrix& other)
	{
		if (*this->n == other.n && *this->m == other.m)
		{
			for (int i = 0; i < *this->n; i++)
				for (int j = 0; j < *this->m; j++)
					*this[i][j] = *this[i][j] - other.mat[i][j];
			return *this;
		}
		else
		{
			cout << "выполнить операцию невозможно по правилу сложения матриц" << endl;
			return 9999;
		}
	}

	/*void set(unsigned int i, unsigned int j, T value)
	{
		mat[i][j] = value;
	}*/

	Matrix operator*(const Matrix& other)
	{
		Matrix<T> result(n, other.m);
		if (m == other.n)
		{
			for (int i = 0; i < n; i++)
				for (int j = 0; j < m; j++)
				{
					T sum = 0;
					for (int q = 0; q < m; q++)
						sum += mat[i][q] * other.mat[q][j];
					result.mat[i][j] = sum;
				}
			return result;
		}
		else
		{
			cout << "выполнить умножение невозможно по правилу умножения матриц" << endl;
			Matrix Error(1, 1);
			Error.mat[0][0] = 9999;
			return Error;
		}
	}

	Matrix operator*=(const Matrix& other)
	{
		Matrix result = *this * other;
		*this = result;
		return *this;
	}

	//умножение матрицы на число (свой оператор)
	Matrix operator*(int a) {
		Matrix<T> result(n,m);
		for (int i = 0; i < n; i++)
		{
			for (int j = 0; j < m; j++)
			{
				T sum = 0;
				sum += mat[i][j] * a;
				result.mat[i][j] = sum;
			}
		}
		return result;
	}

	T Determinant()
	{
		if (n == m)
		{
			if (n == 1) { return mat[0][0]; }
			if (n == 2) { return mat[0][0] * mat[1][1] - mat[0][1] * mat[1][0]; }
			if (n == 3) { return (mat[0][0] * (mat[1][1] * mat[2][2] - mat[1][2] * mat[2][1]) - mat[0][1] * (mat[1][0] * mat[2][2] - mat[2][0] * mat[1][2]) + mat[0][2] * (mat[1][0] * mat[2][1] - mat[2][0] * mat[1][1])); }
		}
		else
		{
			cout << "Невозможно вычислить определитель матрицы ввиду некорректных размеров" << endl;
			return 9999;
		}
	}

	void destroyer(int k)
	{
		if ((k >= 0) && (k <= n - 1))
		{
			for (int i = 0; i < n; i++)
			{
				for (int j = 0; j < m; j++)
				{
					if (i < k)
						mat[i][j] = mat[i][j];
					else if (i > k)
						mat[i - 1][j] = mat[i][j];
				}
				if (i == n - 1)
				{
					n = n - 1;
					delete[] mat[i];
				}
			}
		}
	}

};


int main()
{
	setlocale(LC_ALL, "RU");
	Matrix<int> m1(3, 3);
	Matrix<int> m2(3, 3);
	Matrix<int> M4(3, 2);
	cout << "введите первую матрицу: " << endl;
	cin >> m1;
	cout << "введите вторую матрицу: " << endl;
	cin >> m2;
	Matrix<int> m3 = m1 + m2;
	cout << "сумма матриц: " << endl;
	cout << m3 << endl;

	m3 = m3 - m1;
	cout << "разность третьей матрицы и первой " << endl;
	cout << m3 << endl;
	m3 += m1;
	cout << "+= m1: " << endl;
	cout << m3 << endl;
	m3 = m1 * m2;
	cout << "2 умноженные матрицы " << endl;
	cout << m3 << endl;
	
	int D = m1.Determinant();
	cout << "Определитель = " << D << endl;

	m2.destroyer(1);
	cout << "вырезанная матрица #2 " << endl;
	cout << m2 << endl;

	Matrix<double> doubles1(3, 2);
	Matrix<double> doubles2(3, 2);
	Matrix<double> doublesSUM(3, 2);

	cout << "первая матрица даббл " << endl;
	cin >> doubles1;
	cout << "вторая матрица даббл " << endl;
	cin >> doubles2;
	doublesSUM = doubles1 + doubles2;
	cout << "Сумма " << endl;
	cout << doublesSUM << endl;

	Matrix<bool> TF(2, 3);
	Matrix<bool> TF2(2, 3);
	cout << "первая булева матрица " << endl;
	cin >> TF;
	cout << "вторая булева матрица " << endl;
	cin >> TF2;
	Matrix<bool> TFSUM(2, 3);
	TFSUM = TF + TF2;
	cout << "Сумма булевых " << endl;
	cout << TFSUM << endl;
}
