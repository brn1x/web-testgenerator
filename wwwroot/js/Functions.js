var listElement = document.querySelector('#app ul');
var inputElement = document.querySelector('#app textarea');
var buttonElement = document.querySelector('#app button');

var todos = [];

function renderTodos() {
    listElement.innerHTML = '';

    for (todo of todos) {
        var todoElement = document.createElement('li');
        var todoText = document.createTextNode(todo);

        var linkElement = document.createElement('a');

        linkElement.setAttribute('href', '#');

        var pos = todos.indexOf(todo);
        linkElement.setAttribute('onclick', 'deleteTodo(' + pos + ')');

        var linkText = document.createTextNode('Delete');

        var radioBtn = document.createElement('input');
        radioBtn.setAttribute('type', 'radio');
        radioBtn.setAttribute('name', 'correctAnswer');

        linkElement.appendChild(linkText);

        radioBtn.setAttribute('style', 'margin-left:10px;')
        //linkElement.setAttribute('style', 'margin-left:10px;')
        linkElement.setAttribute('class', 'btn btn-danger form-control')

        todoElement.appendChild(todoText);
        todoElement.appendChild(radioBtn);
        todoElement.appendChild(linkElement);
        



        listElement.appendChild(todoElement);
    }
}

renderTodos();

function addTodo() {
    var todoText = inputElement.value;

    todos.push(todoText);
    inputElement.value = '';
    renderTodos();
}

buttonElement.onclick = addTodo;

function deleteTodo(pos) {
    todos.splice(pos, 1);

    renderTodos();
}