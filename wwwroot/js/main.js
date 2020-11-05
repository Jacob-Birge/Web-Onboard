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