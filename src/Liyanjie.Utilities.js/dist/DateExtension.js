Date.prototype.format = function (format, weekDisplay) {
    if (weekDisplay === void 0) { weekDisplay = {}; }
    var o = {
        "M{1,2}": this.getMonth() + 1,
        "d{1,2}": this.getDate(),
        "h{1,2}": this.getHours() % 12 == 0 ? 12 : this.getHours() % 12,
        "H{1,2}": this.getHours(),
        "m{1,2}": this.getMinutes(),
        "s{1,2}": this.getSeconds(), //秒
    };
    var w = {
        "0": weekDisplay.sun || "星期日",
        "1": weekDisplay.mon || "星期一",
        "2": weekDisplay.tue || "星期二",
        "3": weekDisplay.wed || "星期三",
        "4": weekDisplay.thu || "星期四",
        "5": weekDisplay.fri || "星期五",
        "6": weekDisplay.sat || "星期六"
    };
    if (/(y+)/.test(format)) {
        format = format.replace(RegExp.$1, this.getFullYear().toString().substr(4 - RegExp.$1.length));
    }
    if (/(d{3,4})/.test(format)) {
        format = format.replace(RegExp.$1, w[this.getDay().toString()]);
    }
    for (var k in o) {
        if (new RegExp("(" + k + ")").test(format)) {
            var value = RegExp.$1.length === 1
                ? (o[k])
                : ("00" + o[k]).substr(o[k].toString().length);
            format = format.replace(RegExp.$1, value);
        }
    }
    if (/(f{1,3})/.test(format)) {
        format = format.replace(RegExp.$1, this.getMilliseconds().toString().substr(3 - RegExp.$1.length));
    }
    return format;
};
Date.prototype.addMillionSeconds = function (millionSeconds) {
    var date = new Date(this.getTime());
    date.setMilliseconds(date.getMilliseconds() + millionSeconds);
    return date;
};
Date.prototype.addSeconds = function (seconds) {
    var date = new Date(this.getTime());
    date.setSeconds(this.getSeconds() + seconds);
    return date;
};
Date.prototype.addMinutes = function (minutes) {
    var date = new Date(this.getTime());
    date.setMinutes(date.getMinutes() + minutes);
    return date;
};
Date.prototype.addHours = function (hours) {
    var date = new Date(this.getTime());
    date.setHours(date.getHours() + hours);
    return date;
};
Date.prototype.addDays = function (days) {
    var date = new Date(this.getTime());
    date.setDate(date.getDate() + days);
    return date;
};
Date.prototype.addMonths = function (months) {
    var date = new Date(this.getTime());
    date.setMonth(date.getMonth() + months);
    return date;
};
Date.prototype.addYears = function (years) {
    var date = new Date(this.getTime());
    date.setFullYear(date.getFullYear() + years);
    return date;
};
//# sourceMappingURL=DateExtension.js.map