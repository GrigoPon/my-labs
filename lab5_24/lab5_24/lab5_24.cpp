#include <iostream>
#include <ostream>

using namespace std;



class R_Complex {
    float _r;
    float _y;
public:
    R_Complex() {
        _r = 1;
        _y = 1;
    }

    R_Complex(float r, float y) {
        _r = r;
        _y = y;
    }

    R_Complex(const R_Complex& p) {
        _r = p._r;
        _y = p._y;
    }
    
    R_Complex& operator=(R_Complex& p) {
        this->_r = p._r;
        this->_y = p._y;
        return *this;
    }


    R_Complex& operator++(int value) {
        R_Complex temp(*this);
        _r++;
        return temp;
    }

    R_Complex& operator++() {
        _r++;
        return *this;
    }

    R_Complex& operator*(const R_Complex& a) {
        R_Complex temp(this->_r * a._r, this->_y + a._y);
        return temp;
    }

    R_Complex& operator +(const R_Complex& a) {
        R_Complex temp(sqrt(this->_r * this->_r + a._r * a._r + this->_r * a._r * cos(abs(this->_y - a._y))), _y);
        return temp;
    }

    R_Complex& operator +=(const R_Complex& a) {
        _r = sqrt(this->_r * this->_r + a._r * a._r + this->_r * a._r * cos(abs(this->_y - a._y)));
        _y = (this->_y + a._y) / 2;
        return *this;
    }

    R_Complex& operator*=(const R_Complex& a) {
        _r = this->_r * a._r;
        _y = this->_y + a._y;
        return *this;
    }


    float GetR() {
        return this->_r;
    }
    float GetY() {
        return this->_y;
    }


    ~R_Complex() {
        cout << "class deleted" << endl;
    }
};

float ScalarC(float a, float b) {
    a.GetR* b.GetR* cos(abs(a.GetY - b.GetY));
}



ostream& operator<< (ostream& os, R_Complex& a) {
    //return os << a.get << a.get << ......
}

istream& operator >> (istream& in, R_Complex& a) {
    //return in >> ...........
}




int main()
{
    
}

