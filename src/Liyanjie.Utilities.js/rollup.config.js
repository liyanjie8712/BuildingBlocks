import typescript from "rollup-plugin-typescript";

let config = {
    input: "./src/index.ts",
    output: {
        file: "bundles/liyanjie.utilities.umd.js",
        name: "liyanjie.utilities",
        format: "umd"
    },
    plugins: [
        typescript()
    ]
};
export default config;