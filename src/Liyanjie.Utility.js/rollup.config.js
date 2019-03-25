import typescript from "rollup-plugin-typescript";

let config = {
    input: "./src/index.ts",
    output: {
        file: "bundles/liyanjie.utility.umd.js",
        name: "liyanjie.utility",
        format: "umd"
    },
    plugins: [
        typescript()
    ]
};
export default config;