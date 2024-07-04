$(function () {
    var PlaceHolderElement = $('#PlaceHolderHere');

    // Event delegation for dynamically added buttons
    $(document).on('click', 'button[data-bs-toggle="ajax-modal"]', function (event) {
        var url = $(this).data('url');
        var decodedUrl = decodeURIComponent(url);
        $.get(decodedUrl).done(function (data) {
            PlaceHolderElement.html(data);
            PlaceHolderElement.find('.modal').modal('show');
        });
    });

    PlaceHolderElement.on('submit', 'form', function (event) {
        event.preventDefault();
        var form = $(this);
        var actionUrl = form.attr('action');
        var sendData = form.serialize();

        $.post(actionUrl, sendData).done(function (data) {
            if (data.success) {
                // Clear the modal content
                form.closest('.modal').modal('hide');

                // Check if the contact already exists in the list
                var existingContactItem = $(`#contactList .contact-item[data-id="${data.contact.contactId}"]`);

                if (existingContactItem.length > 0) {
                    // Update existing contact in the list
                    existingContactItem.find('li').html(`${data.contact.firstName} ${data.contact.lastName} <br /> ${data.contact.email}`);
                } else {
                    // Append new contact to the list if not found
                    var newContactHtml = `<div class="d-flex contact-item" style="margin-bottom:8px;" data-id="${data.contact.contactId}" data-email="${data.contact.email}">
                                            <div class="col-sm-1">
                                                <button data-bs-toggle="ajax-modal" data-bs-target="#editContact" data-url="/Contact/Edit/${data.contact.contactId}" style="border: none; padding: 0; background-color: transparent;" class="d-block"><i class="fa-solid fa-pen-to-square"></i></button>
                                                <button class="delete-contact" data-id="${data.contact.contactId}" style="border: none; padding: 0; background-color: transparent; color: indianred;"><i class="fa-solid fa-trash"></i></button>
                                            </div>
                                            <div class="col-10">
                                                <li>${data.contact.firstName} ${data.contact.lastName} <br /> ${data.contact.email}</li>
                                            </div>
                                            <div class="col-sm-2">
                                                <button class="toggle-favorite" data-id="${data.contact.contactId}" data-favorite="${data.contact.isFavorite.toString().toLowerCase()}" style="border: none; padding: 0; background-color: transparent; color: ${data.contact.isFavorite ? 'goldenrod' : 'gray'}"><i class="fa-regular fa-star"></i></button>
                                            </div>
                                        </div>`;

                    $('#contactList').append(newContactHtml);
                }
            } else {
                // Handle errors or validation messages
                var newBody = $('.modal-body', data);
                PlaceHolderElement.find('.modal-body').replaceWith(newBody);
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            console.error('Error:', textStatus, errorThrown);
        });
    });

    $(document).on('click', '.delete-contact', function (event) {
        event.preventDefault();
        if (confirm("Are you sure you want to delete this contact?")) {
            var button = $(this);
            var contactId = button.data('id');
            var deleteUrl = "/Contact/Delete/" + contactId;

            $.post(deleteUrl).done(function (response) {
                if (response.success) {
                    // Remove the deleted contact from the list
                    $(`#contactList .contact-item[data-id="${contactId}"]`).remove();
                } else {
                    alert("Error deleting contact.");
                }
            });
        }
    });

    $(document).on('click', '.toggle-favorite', function (event) {
        event.preventDefault();
        var button = $(this);
        var contactId = button.data('id');
        var currentFavorite = button.data('favorite') === 'true';
        var newFavorite = !currentFavorite;
        var sendData = {
            contactId: contactId,
            isFavorite: newFavorite
        };

        $.ajax({
            url: '/Contact/ToggleFavorite',
            method: 'POST',
            data: sendData,
            success: function (response) {
                if (response.success) {
                    button.data('favorite', newFavorite.toString().toLowerCase());
                    button.css('color', newFavorite ? 'goldenrod' : 'gray');
                } else {
                    alert('Error toggling favorite status.');
                }
            },
            error: function () {
                alert('Error toggling favorite status.');
            }
        });
    });
});
