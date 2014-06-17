/// <reference path="../jquery-1.8.2.js" />
/// <reference path="../jquery.mobile-1.2.0.js" />

(function () {
    var error;
    
    function init() {
        $('div[data-role="navbar"] li#RoleMenu a').addClass('ui-btn-active');
        $('div[data-role="navbar"] li#RoleMenu a').addClass('ui-state-persist');
        $('#menu-new').bind('click', menuNewClicked);
        $('#createNewRole').bind('click', createNewRole);
    }

    function loadRoles() {
        $.getJSON('/api/roles', null, function (data, status, xhr)
        {
            console.log(status);
            console.log(xhr);
            if (status === 'success') {
                for (var index in data) {
                    var html = "<li><a href='#'>" + data[index].RoleName + "<a/></li>";
                    $('ul[data-role="listview"]').append($(html));
                }
                $('ul[data-role="listview"]').listview('refresh');
                $('#div-roles').show();
            }
            else {
                $('#div-error').html(xhr.responseText);
                $('#div-error').show();
            }
        });
    }

    function menuNewClicked(e) {
        if ($(this).data('menuname') === 'new') {
            newRole();
        } else {
            cancelNewRole();
        }
        e.preventDefault();
    }

    function newRole() {
        $('#div-error').hide();
        $('#div-roles').hide();
        $('#rolename').val('');
        $('#div-new').show();
        $('#menu-new').data('menuname', 'cancel');
        $('#menu-new').data('icon', 'back');
        $('#menu-new span.ui-icon').removeClass('ui-icon-plus');
        $('#menu-new span.ui-icon').addClass('ui-icon-back');
        $('#menu-new span.ui-btn-text').text('Back');
    }

    function cancelNewRole() {
        $('#div-error').hide();
        $('#div-new').hide();
        $('#div-roles').show();
        $('#menu-new').data('menuname', 'new');
        $('#menu-new').data('icon', 'plus');
        $('#menu-new span.ui-icon').removeClass('ui-icon-back');
        $('#menu-new span.ui-icon').addClass('ui-icon-plus');
        $('#menu-new span.ui-btn-text').text('New');
    }

    function createNewRole() {
        if ($('#rolename').val().trim()) {
            $.ajax('/api/roles', {
                type: 'POST',
                dataType: 'JSON',
                data: { RoleName: $('#rolename').val().trim() },
                success: function (data,status,xhr) {
                    console.log(arguments);
                    $('div-new-message').empty();
                    $('#menu-new span.ui-icon').removeClass('ui-icon-back');
                    $('#menu-new span.ui-icon').addClass('ui-icon-plus');
                    $('#menu-new span.ui-btn-text').text('New');
                    $('#div-error').hide();
                    $('#div-new').hide();
                    $('#div-roles').show();
                    loadRoles();
                },
                error: function (xhr, status, msg) {
                    error = JSON.parse(xhr.responseText);
                    $('#div-new-message').html(error.ExceptionMessage);
                }
            });
        }
    }

    $(function () {
        init();
        loadRoles();
    });
}());