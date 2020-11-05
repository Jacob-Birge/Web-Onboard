$(function () { // Global on-laod functions.

});

var hexDigits = new Array
    ("0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f");

function rgb2hex(rgb) {
    rgb = rgb.match(/^rgb\((\d+),\s*(\d+),\s*(\d+)\)$/);
    return "#" + hex(rgb[1]) + hex(rgb[2]) + hex(rgb[3]);
}

function hex(x) {
    return isNaN(x) ? "00" : hexDigits[(x - x % 16) / 16] + hexDigits[x % 16];
}

Array.prototype.move = function (from, to) {
    this.splice(to, 0, this.splice(from, 1)[0]);
};

function create_listbox(id, click_event) {
    var elm = $("#" + id);
    var new_id = id + "-new";
    var result = "<div class='list-box' id='" + new_id + "'>";
    var i = 0;

    elm.find("option").each(function () {
        result += "<div class='list-option' name='" + $(this).val() + "'";
        if ($(this).attr("selected") == "selected") {
            if (!(typeof lbColors === 'undefined')) {
                result += " style='background-color: " + hexToRgbA(lbColors[i], 1) + "; border-color: " + hexToRgbA(lbColors[i], 1) + "; color: #fff;'"
            }
            else {
                result += " style='background-color: #499dff; border-color: #499dff; color: #fff;'";
            }
        }
        else {
            if (!(typeof lbColors === 'undefined')) {
                result += " style='background-color: " + hexToRgbA(lbColors[i], 0.3) + "; border-color: " + hexToRgbA(lbColors[i], 1) + ";'"
            }
        }
        result += ">" + $(this).html() + "</div>";

        i++;
    });

    result += "</div>";
    $(result).insertAfter(elm);
    //elm.css("display", "none");

    $("#" + new_id).find(".list-option").each(function () {
        $(this).click(function () {
            LoadHtml(click_event, $(this));
        });
    });

    return true;
}

function LoadHtml(func, obj) {
    func(obj);
}

function enable_btn(id, cls) {
    var btn = $("#" + id);

    btn.prop('disabled', false);
    btn.removeClass();
    btn.addClass(cls);
}

function disable_btn(id) {
    var btn = $("#" + id);

    btn.prop('disabled', true);
    btn.removeClass();
    btn.addClass('cbutton-1');
}

function hexToRgbA(hex, o) {
    var c;
    if (/^#([A-Fa-f0-9]{3}){1,2}$/.test(hex)) {
        c = hex.substring(1).split('');
        if (c.length == 3) {
            c = [c[0], c[0], c[1], c[1], c[2], c[2]];
        }
        c = '0x' + c.join('');
        return 'rgba(' + [(c >> 16) & 255, (c >> 8) & 255, c & 255].join(',') + ',' + o + ')';
    }
    throw new Error('Bad Hex');
}

function getUrlParameter(sParam) {
    var sPageURL = decodeURIComponent(window.location.search.substring(1)),
        sURLVariables = sPageURL.split('&'),
        sParameterName,
        i;

    for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i].split('=');

        if (sParameterName[0] === sParam) {
            return sParameterName[1] === undefined ? true : sParameterName[1];
        }
    }
};

function set_file_upload(fu, results_lbl, preview, txt) {
    fu.change(function (e) {
        if (this.files && this.files[0]) {
            if (this.files[0].size > 250000) {
                results_lbl.html("<img src='/Media/Images/error.png' /> File size too large. " + Math.ceil(this.files[0].size / 1000) + " kb > 250 kb.");
                results_lbl.addClass('error-box');
                results_lbl.removeClass('warning-box success-box');

                $(this).val('');
            }
            else {
                var reader = new FileReader();
                reader.onload = function (e) {
                    preview.attr('src', e.target.result);
                    preview.css('display', 'block');
                    txt.val(preview.attr('src'));
                }

                reader.readAsDataURL(this.files[0]);

                results_lbl.html("<img src='/Media/Images/check.png' /> File loaded successfully: " + Math.ceil(this.files[0].size / 1000) + " kb.");
                results_lbl.addClass('success-box');
                results_lbl.removeClass('warning-box error-box');
            }
        }
        else {
            preview.css('display', 'none');
        }

        if ($(this).val() !== '') {
            if (this.files && this.files.length > 1) {
                $(this).next().html('<img src="/Media/Images/download.png" />' + $(this).attr('data-multiple-caption').replace('{count}', this.files.length));
            }
            else {
                $(this).next().html('<img src="/Media/Images/download.png" />' + $(this).val().split('\\').pop());
            }

            $(this).next().css('background-color', '#ceffcc');
            $(this).next().css('border', 'solid 1px #2aa825');

            $(this).next().unbind('hover');
            $(this).next().hover(function (e) {
                $(this).css('background-color', '#a1ff9e');
            }, function () {
                $(this).css('background-color', '#ceffcc');
            });
        }
        else {
            $(this).next().css('background-color', '#ddd');
            $(this).next().css('border', 'solid 1px #bbb');
            $(this).next().html('<img src="/Media/Images/download.png" />Choose file...')

            $(this).next().unbind('hover');
            $(this).next().hover(function (e) {
                $(this).css('background-color', '#ccc');
            }, function () {
                $(this).css('background-color', '#ddd');
            });
        }
    });
}

function getFormattedDate(date) {
    var year = date.getFullYear();

    var month = (1 + date.getMonth()).toString();
    month = month.length > 1 ? month : '0' + month;

    var day = date.getDate().toString();
    day = day.length > 1 ? day : '0' + day;

    return month + '/' + day + '/' + year;
}

function isValidDate(d) {
    return d instanceof Date && !isNaN(d);
}

function getWeekNumber(d) {
    d = new Date(Date.UTC(d.getFullYear(), d.getMonth(), d.getDate()));
    d.setUTCDate(d.getUTCDate() + 4 - (d.getUTCDay() || 7));
    var yearStart = new Date(Date.UTC(d.getUTCFullYear(), 0, 1));
    var weekNo = Math.ceil((((d - yearStart) / 86400000) + 1) / 7);
    return [d.getUTCFullYear(), weekNo];
}

function getDateOfISOWeek(w, y) {
    var simple = new Date(y, 0, 1 + (w - 1) * 7);
    var dow = simple.getDay();
    var ISOweekStart = simple;
    if (dow <= 4)
        ISOweekStart.setDate(simple.getDate() - simple.getDay() + 1);
    else
        ISOweekStart.setDate(simple.getDate() + 8 - simple.getDay());
    return ISOweekStart;
}

function ordinal_suffix_of(i) {
    var j = i % 10,
        k = i % 100;
    if (j == 1 && k != 11) {
        return i + "st";
    }
    if (j == 2 && k != 12) {
        return i + "nd";
    }
    if (j == 3 && k != 13) {
        return i + "rd";
    }
    return i + "th";
}


var changes_made = false;
var changes_pending = false;
var edit_mode = false;
var delete_enabled = false;

var name_bank = "";
var resparty_bank = "";
var desc_bank = "";
var scheme_bank = "";

function event_manager_onload() {
    alert("we make it!");

    var toolbarOptions = [
        ['bold', 'italic', 'underline', 'strike'],        // toggled buttons
        ['blockquote', 'code-block'],

        //[{ 'header': 1 }, { 'header': 2 }],               // custom button values
        [{ 'list': 'ordered' }, { 'list': 'bullet' }],
        [{ 'script': 'sub' }, { 'script': 'super' }],      // superscript/subscript
        [{ 'indent': '-1' }, { 'indent': '+1' }],          // outdent/indent
        //[{ 'direction': 'rtl' }],                         // text direction

        [{ 'size': ['small', false, 'large', 'huge'] }],  // custom dropdown
        [{ 'header': [1, 2, 3, 4, 5, 6, false] }],

        [{ 'color': [] }, { 'background': [] }],          // dropdown with defaults from theme
        //[{ 'font': [] }],
        [{ 'align': [] }],
        ['link', 'image']//,

        //['clean']
    ];

    var quill = new Quill('#editor', {
        modules: {
            toolbar: toolbarOptions
        },
        theme: 'snow'
    });

    $('#txtSearchItemsClient').keypress(function (e) {
        if (e.keyCode == 13) {
            e.preventDefault();

            $("#cphBody_txtSearchItems").val($(this).val());
            $('#cphBody_btnSearchItems').trigger('click');
        }
    });

    search_autocomplete();
    set_listbox();
    set_scheme_dd();

    //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
    //    if (changes_pending) {
    //        var btnDelete = $("#btnDelete_Client");

    //        disable_btn('cphBody_btnSave');

    //        set_banks();

    //        $("#editor").find(".ql-editor").html(desc_bank);

    //        if (edit_mode) {
    //            btnDelete.css('display', 'inline-block');
    //            enable_btn('btnDelete_Client', 'cbutton-3');
    //            delete_enabled = true;
    //        }
    //        else {
    //            disable_btn('btnDelete_Client');
    //        }

    //        changes_pending = false;
    //        changes_made = false;
    //    }

    //    search_autocomplete();
    //    set_listbox();
    //    set_scheme_dd();
    //    add_listeners();

    //    $('#txtSearchItemsClient').keypress(function (e) {
    //        if (e.keyCode == 13) {
    //            e.preventDefault();

    //            $("#cphBody_txtSearchItems").val($(this).val());
    //            $('#cphBody_btnSearchItems').trigger('click');
    //        }
    //    });
    //});

    $("#editor").find(".ql-editor").html($("#cphBody_txtDescription").val());
    if ($("#cphBody_txtTitle").val() != "") {
        enable_btn('btnDelete_Client', 'cbutton-3');
        delete_enabled = true;
        edit_mode = true;
        set_banks();
        add_listeners();
    }
}

function set_listbox() {
    var click_event = function (obj) {
        var id = "cphBody_lbEvents";
        var elm = $("#" + id);
        var new_id = id + "-new";
        var old_selected_value = 0;

        if (changes_made) {
            var confirm = $('<div title="WARNING!" style="text-align: left;"><p><span class="ui-icon ui-icon-alert" style="float:left; margin:12px 12px 20px 0;"></span>Your current changes have not been saved. Any changes you have made will be lost.</p></div>').dialog({
                resizable: false,
                height: "auto",
                width: 400,
                modal: true,
                buttons: {
                    "Don't Save": function () {
                        changes_pending = true;
                        edit_mode = true;

                        obj.css("background-color", obj.css("background-color").replace("0.3", "1"));

                        obj.css("color", "#fff");

                        $("#" + id).find("option").removeAttr("selected");
                        $("#" + id).find("option").eq(obj.index()).attr('selected', 'selected');

                        $("#cphBody_btnLoadEvent").trigger("click");

                        confirm.dialog('close');
                    },
                    "Cancel": function () {
                        confirm.dialog('close');
                    }
                }
            });
        }
        else {
            changes_pending = true;
            edit_mode = true;

            obj.css("background-color", obj.css("background-color").replace("0.3", "1"));
            obj.css("color", "#fff");

            $("#" + id).find("option").removeAttr("selected");
            $("#" + id).find("option").eq(obj.index()).attr('selected', 'selected');

            $("#cphBody_btnLoadEvent").trigger("click");
        }
    };

    if (create_listbox("cphBody_lbEvents", click_event)) {
        $("#cphBody_lbEvents-new").find(".list-option").hover(function () {
            $(this).css("background-color", $(this).css("background-color").replace("0.3", "0.4"));
        }, function () {
            $(this).css("background-color", $(this).css("background-color").replace("0.4", "0.3"));
        });
    }
}

function set_banks() {
    name_bank = $("#cphBody_txtTitle").val();
    resparty_bank = $("#cphBody_txtResParty").val();
    desc_bank = $("#cphBody_txtDescription").val();
    scheme_bank = $("#cphBody_ddColorSchemes").val();
}


function add_listeners() {
    if (edit_mode) {
        var txtTitle = $("#cphBody_txtTitle");
        var txtResParty = $("#cphBody_txtResParty");
        var txtDescription = $("#editor").find(".ql-editor");

        txtTitle.unbind();
        txtResParty.unbind();
        txtDescription.unbind();

        txtTitle.keyup(function () { check_changed(); });
        txtResParty.keyup(function () { check_changed(); });
        txtDescription.keyup(function () { check_changed(); });
    }
}

function check_changed() {
    if (edit_mode) {
        var txtTitle = $("#cphBody_txtTitle");
        var txtResParty = $("#cphBody_txtResParty")
        var txtDescription = $("#editor").find(".ql-editor");
        var ddScheme = $("#cphBody_ddColorSchemes");

        if (txtTitle.val() != name_bank || txtDescription.html() != desc_bank || txtResParty.val() != resparty_bank || ddScheme.val() != scheme_bank) {
            changes_made = true;
            enable_btn('cphBody_btnSave', 'cbutton-2');
        }
        else {
            changes_made = false;
            disable_btn('cphBody_btnSave');
        }
    }
}

function check_save() {
    var txtTitle = $("#cphBody_txtTitle");
    changes_pending = true;
    edit_mode = true;

    $("#cphBody_lbEvents-new .list-option[name=" + $("#cphBody_lbEvents").val() + "]").html(txtTitle.val());
    txtTitle.val($("#cphBody_lbEvents").val() + "{or}" + txtTitle.val());
    $("#cphBody_txtDescription").val($("#editor").find(".ql-editor").html());

    return true;
}

function check_save_as() {
    changes_pending = true;
    edit_mode = true;

    $("#cphBody_txtDescription").val($("#editor").find(".ql-editor").html());
    return true;
}

function confirm_delete() {
    if (delete_enabled) {
        var confirm = $('<div title="WARNING!" style="text-align: left;"><p><span class="ui-icon ui-icon-alert" style="float:left; margin:12px 12px 20px 0;"></span>Are you sure you want to delete the event: ' + $("#cphBody_lbEvents-new .list-option[name='" + $("#cphBody_lbEvents").val() + "']").html() + '?</p></div>').dialog({
            resizable: false,
            height: "auto",
            width: 400,
            modal: true,
            buttons: {
                'Delete': function () {
                    edit_mode = false;
                    $("#editor").find(".ql-editor").html("");
                    $("#cphBody_btnDelete").trigger("click");
                    confirm.dialog('close');
                },
                "Cancel": function () {
                    confirm.dialog('close');
                }
            }
        });
    }
}

function search_autocomplete() {
    var events = [];
    $("#cphBody_lbAllEvents option").each(function () {
        events.push($(this).html());
    });

    $("#txtSearchItemsClient").autocomplete({
        source: events,
        select: function (event, ui) {
            $("#cphBody_txtSearchItems").val($(this).val());
            $('#cphBody_btnSearchItems').trigger('click');
        }
    });
}

function set_scheme_dd() {
    var click_event = function (obj) {
        $("#cphBody_ddColorSchemes-new-text").html($(obj).html() + " <span style='font-size: 10px; margin: 0 0 0 5px;'>&#x25BC;</span>");
        $(obj).parent().parent().css('background-color', $(obj).html());

        $("#cphBody_ddColorSchemes").find("option").removeAttr("selected");
        $("#cphBody_ddColorSchemes").find("option").eq(obj.index()).attr('selected', 'selected');

        $("#cphBody_ddColorSchemes-new .drop-list").css("display", "none");

        check_changed();
    };

    var elm = $("#cphBody_ddColorSchemes");
    var new_id = "cphBody_ddColorSchemes-new";
    var result = "<div class='drop-down' id='" + new_id + "' style='background-color: " + elm.find("option:selected").html() + ";'><div id='" + new_id + "-text'>" + elm.find("option:selected").html() + " <span style='font-size: 10px; margin: 0 0 0 5px;'>&#x25BC;</span></div><div class='drop-list'>";

    elm.find("option").each(function () {
        result += "<div class='drop-option' name='" + $(this).val() + "' style='background-color: " + $(this).html() + ";'>" + $(this).html() + "</div>";
    });

    result += "</div></div>";
    $(result).insertAfter(elm);
    //elm.css("display", "none");

    $("#" + new_id).find(".drop-option").each(function () {
        $(this).click(function () {
            LoadHtml(click_event, $(this));
        });

        var hold_hex = "";
        $(this).hover(function () {
            hold_hex = $(this).css("background-color")
            $(this).css("background-color", hexToRgbA($(this).html(), 0.8));
        }, function () {
            $(this).css("background-color", hold_hex);
        });
    });


    $("#cphBody_ddColorSchemes-new").hover(function () {
        $("#" + new_id).find(".drop-list").css("display", "block");
    }, function () {
        $("#" + new_id).find(".drop-list").css("display", "none");
    });
}