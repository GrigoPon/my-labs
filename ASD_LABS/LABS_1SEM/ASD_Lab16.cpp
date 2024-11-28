#include <iostream>
#include <stack>
#include <string>
#include <sstream>

using namespace std;

struct TreeNode {
    int value;
    TreeNode* Left;
    TreeNode* Right;
    TreeNode(int x) : value(x), Left(nullptr), Right(nullptr) {}
};

// Функция для создания дерева
TreeNode* parseTree(istringstream& tree) {
    if (tree.peek() == ' ') {
        tree.get(); // Пропустить пробел
    }

    if (tree.peek() == ')' || tree.peek() == ',' || tree.peek() == EOF) {
        return nullptr;
    }

    int value;
    tree >> value;
    TreeNode* node = new TreeNode(value);

    if (tree.peek() == ' ') {
        tree.get(); // Пропускать пробелы
    }

    if (tree.peek() == '(') {
        tree.get(); // Пропустить '('
        node->Left = parseTree(tree);

        if (tree.peek() == ',') {
            tree.get(); // Пропустить ','
            node->Right = parseTree(tree);
        }
        tree.get(); // Пропустить ')'
    }

    return node;
}

// Прямой обход
string preorderTraversal(TreeNode* root) {
    if (!root) return "";

    stack<TreeNode*> stack;
    stack.push(root); // Добавляем
    string result;

    while (!stack.empty()) {
        TreeNode* node = stack.top(); // Достаем
        stack.pop(); // Удаляем
        result += to_string(node->value) + " "; // Добавляем в результат

        // Сначала помещаем правый узел, чтобы левый узел был обработан первым
        if (node->Right) {
            stack.push(node->Right);
        }
        if (node->Left) {
            stack.push(node->Left);
        }
    }

    return result;
}

int main() {
    setlocale(0, "RU");

    string Example = "8 (3 (1, 6 (4, 7)), 10 (, 14 (13,)))";
    istringstream tree(Example);
    TreeNode* Root = parseTree(tree);

    string result = preorderTraversal(Root);
    cout << "Результат обхода: " << result << endl;

    return 0;
}