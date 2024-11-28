#include <iostream>
#include <string>
#include <sstream>

using namespace std;

struct TreeNode {

    int value;
    TreeNode* Left;
    TreeNode* Right;

    TreeNode(int x) : value(x), Left(nullptr), Right(nullptr) {}
};

// Cоздание дерева
TreeNode* parseTree(istringstream& tree) {
    if (tree.peek() == ' ') {
        tree.get();
    }

    if (tree.peek() == ')' || tree.peek() == ',' || tree.peek() == EOF) {
        return nullptr;
    }

    int value;
    tree >> value; // считываем значение
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


// Корень, левый узел, правый узел
void preorderTraversal(TreeNode* node) {
    if (!node) return;
    cout << node->value << " ";
    preorderTraversal(node->Left);
    preorderTraversal(node->Right);
}


// Левый узел, корень, правый узел
void inorderTraversal(TreeNode* node) {
    if (!node) return;
    inorderTraversal(node->Left);
    cout << node->value << " ";
    inorderTraversal(node->Right);
}

// Левый узел, правый узел, корень
void postorderTraversal(TreeNode* node) {
    if (!node) return;
    postorderTraversal(node->Left);
    postorderTraversal(node->Right);
    cout << node->value << " ";
}

int main() {
    setlocale(0, "RU");
    string Example = "8 (3 (1, 6 (4, 7)), 10 (, 14 (13,)))";
    istringstream tree(Example);
    TreeNode* root = parseTree(tree);

    cout << "Прямой обход: ";
    preorderTraversal(root);
    cout << endl;

    cout << "Центральный обход: ";
    inorderTraversal(root);
    cout << endl;

    cout << "Концевой обход: ";
    postorderTraversal(root);
    cout << endl;

    return 0;
}