interface Number {
    plus(arg: number): number;
    minus(arg: number): number;
    multipy(arg: number): number;
    divide(arg: number): number;
    toCNNumber(upperOrLower?: boolean): string;
    toCN(currency?: boolean): string;
}
