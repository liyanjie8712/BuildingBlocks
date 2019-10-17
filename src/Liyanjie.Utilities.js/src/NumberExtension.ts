interface Number {
    plus(arg: number): number;
    minus(arg: number): number;
    multipy(arg: number): number;
    divide(arg: number): number;
    toCNNumber(upperOrLower?: boolean): string;
    toCN(currency?: boolean): string;
}

Number.prototype.plus = function (arg: number) {
    let r1, r2, m;
    try {
        r1 = this.toString().split('.')[1].length;
    } catch (e) {
        r1 = 0;
    }
    try {
        r2 = arg.toString().split('.')[1].length;
    } catch (e) {
        r2 = 0;
    }
    m = Math.pow(10, Math.max(r1, r2));
    return (this * m + arg * m) / m;
};
Number.prototype.minus = function (arg: number) {
    let r1, r2, m, n;
    try {
        r1 = this.toString().split('.')[1].length;
    } catch (e) {
        r1 = 0;
    }
    try {
        r2 = arg.toString().split('.')[1].length;
    } catch (e) {
        r2 = 0;
    }
    m = Math.pow(10, Math.max(r1, r2));
    n = (r1 >= r2) ? r1 : r2;
    return Number(((this * m - arg * m) / m).toFixed(n));
};
Number.prototype.multipy = function (arg: number) {
    let m = 0, s1 = this.toString(), s2 = arg.toString();
    try {
        m += s1.split('.')[1].length;
    } catch (e) { }
    try {
        m += s2.split('.')[1].length;
    } catch (e) { }
    return Number(s1.replace('.', '')) * Number(s2.replace('.', '')) / Math.pow(10, m)
};
Number.prototype.divide = function (arg: number) {
    let t1 = 0, t2 = 0, r1, r2;
    try {
        t1 = this.toString().split('.')[1].length;
    } catch (e) { }
    try {
        t2 = arg.toString().split('.')[1].length;
    } catch (e) { }
    r1 = Number(this.toString().replace('.', ''));
    r2 = Number(arg.toString().replace('.', ''));
    return (r1 / r2) * Math.pow(10, t2 - t1);
};
Number.prototype.toCNNumber = function (upperOrLower: boolean = false) {
    let i = parseInt(this.toString()), f = '';
    if (i >= 100000000)
        i = 100000000;
    else if (i >= 10000)
        i = 10000;
    else if (i >= 1000)
        i = 1000;
    else if (i >= 100)
        i = 100;
    else if (i >= 10)
        i = 10;
    else if (i < 0)
        i = 0;
    if (i > 100000000)
        i /= 100000000;
    if (upperOrLower) {
        switch (i) {
            case 0:
                f = '零';
                break;
            case 1:
                f = '壹';
                break;
            case 2:
                f = '贰';
                break;
            case 3:
                f = '叁';
                break;
            case 4:
                f = '肆';
                break;
            case 5:
                f = '伍';
                break;
            case 6:
                f = '陆';
                break;
            case 7:
                f = '柒';
                break;
            case 8:
                f = '捌';
                break;
            case 9:
                f = '玖';
                break;
            case 10:
                f = '拾';
                break;
            case 100:
                f = '佰';
                break;
            case 1000:
                f = '仟';
                break;
            case 10000:
                f = '万';
                break;
            case 100000000:
                f = '亿';
                break;
            default:
                break;
        }
    }
    else {
        switch (i) {
            case 0:
                f = '〇';
                break;
            case 1:
                f = '一';
                break;
            case 2:
                f = '二';
                break;
            case 3:
                f = '三';
                break;
            case 4:
                f = '四';
                break;
            case 5:
                f = '五';
                break;
            case 6:
                f = '六';
                break;
            case 7:
                f = '七';
                break;
            case 8:
                f = '八';
                break;
            case 9:
                f = '九';
                break;
            case 10:
                f = '十';
                break;
            case 100:
                f = '百';
                break;
            case 1000:
                f = '千';
                break;
            case 10000:
                f = '万';
                break;
            case 100000000:
                f = '亿';
                break;
            default:
                break;
        }
    }
    return f;
};
Number.prototype.toCN = function (currency: boolean = false) {
    let num = 0,//整数部分
        dec,//小数部分
        output0 = false,//输出0
        s = '',
        unit = function (i) {
            let s;
            switch (i) {
                case 0:
                    s = '角';
                    break;
                case 1:
                    s = '分';
                    break;
                case 2:
                    s = '厘';
                    break;
                default:
                    break;
            }
            return s;
        };
    num = parseInt(this.toString());
    dec = this.minus(num);
    if (num < 0)
        s += '负';
    if (num > 1) {
        let l = num;
        for (let ii = 4; ii >= 0; ii--) {
            let level = Math.pow(10000, ii);
            if (num >= level) {
                l = num % level;
                num = num.divide(level);
                if (num > 19) {
                    let j = 1000;
                    while (num % (j * 10) >= 1) {
                        let tmp = parseInt((num / j).toString());
                        if (tmp != 0) {
                            s += (tmp).toCNNumber(currency);
                            if (j > 1)
                                s += j.toCNNumber(currency);
                            output0 = true;
                        }
                        else if (output0) {
                            s += (0).toCNNumber(currency);
                            output0 = false;
                        }
                        if (j == 1)
                            break;
                        num %= j;
                        j = j.divide(10);
                    }
                }
                else if (num >= 10) {
                    s += (10).toCNNumber(currency);
                    if (num % 10 > 0) {
                        s += (num % 10).toCNNumber(currency);
                        output0 = true;
                    }
                }
                else
                    s += num.toCNNumber(currency);
                if (level > 1)
                    s += level.toCNNumber(currency);
            }
            num = l;
        }
    }
    else
        s += num.toCNNumber(currency);
    if (dec > 0) {
        //处理小数部分
        s += currency ? '圆' : '点';
        if (currency) {
            if (output0) {
                s += (0).toCNNumber(currency);
                output0 = false;
            }
            let i = 0;
            do {
                dec = dec.multipy(10);
                let dd = parseInt(dec.toString());
                dec = dec.minus(dd);
                if (dd > 0) {
                    s += dd.toCNNumber(currency);
                    s += unit(i);
                    output0 = true;
                }
                else if (dec > 0 && output0) {
                    s += (0).toCNNumber(currency);
                    output0 = false;
                }
                i++;
                if (i > 2)
                    break;
            } while (dec > 0);
        }
        else {
            do {
                dec = dec.multipy(10);
                let dd = parseInt(dec.toString());
                dec = dec.minus(dd);
                s += dd.toCNNumber(currency);
            } while (dec > 0)
        }
    }
    else if (currency)
        s += '圆整';
    return s;
};
