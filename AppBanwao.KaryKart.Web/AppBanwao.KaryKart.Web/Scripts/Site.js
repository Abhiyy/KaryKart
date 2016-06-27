function OpenModal(id)
{
    id = "#" + id;
    $(id).modal('show');
}

function CloseModal(id) {
    id = "#" + id;
    $(id).modal('hide');
}